using APPLICATION.DOMAIN.CONTRACTS.SERVICES.USER;
using APPLICATION.DOMAIN.DTOS.REQUEST.USER;
using APPLICATION.DOMAIN.DTOS.RESPONSE.UTILS;
using APPLICATION.DOMAIN.ENUM;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Security.Claims;

namespace APPLICATION.APPLICATION.SERVICES.USER;

public class RoleService : IRoleService
{
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;

    private readonly IMapper _autoMapper;

    public RoleService(RoleManager<IdentityRole<Guid>> roleManager, IMapper autoMapper)
    {
        _roleManager = roleManager;

        _autoMapper = autoMapper;
    }

    #region Create
    /// <summary>
    /// Método responsavel por criar uma nova role.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<ApiResponse<object>> Create(RoleRequest roleRequest)
    {
        Log.Information($"[LOG INFORMATION] - SET TITLE {nameof(RoleService)} - METHOD {nameof(Create)}\n");

        try
        {
            #region User create & set roles & claims
            // Mapper to entity.
            var role = _autoMapper.Map<IdentityRole<Guid>>(roleRequest);

            Log.Information($"[LOG INFORMATION] - Criando nova Role {roleRequest.Name}\n");

            // Create a role in database.
            var response = await _roleManager.CreateAsync(role);

            // Is success enter.
            if (response.Succeeded)
            {
                Log.Information($"[LOG INFORMATION] - Adicionando claims na role {roleRequest.Name}\n");

                // foreach and add claims in request
                roleRequest.Claims.ForEach(claim => _roleManager.AddClaimAsync(role, new Claim(claim.Type, claim.Value)));

                // Response success.
                return new ApiResponse<object>(response.Succeeded, StatusCodes.SuccessCreated, new List<DadosNotificacao> { new DadosNotificacao("Role criado com sucesso.") });
            }
            #endregion

            Log.Information($"[LOG INFORMATION] - Falha ao criar role.\n");

            // Response error.
            return new ApiResponse<object>(response.Succeeded, StatusCodes.ErrorBadRequest, response.Errors.Select(e => new DadosNotificacao(e.Description)).ToList());
        }
        catch (Exception exception)
        {
            Log.Error($"[LOG ERROR] - {exception.InnerException} - {exception.Message}\n");

            // Error response.
            return new ApiResponse<object>(false, StatusCodes.ServerErrorInternalServerError, new List<DadosNotificacao> { new DadosNotificacao(exception.Message) });
        }
    }
    #endregion

    #region Add Claim in role
    /// <summary>
    /// Método responsavel por adicionar uma claim na role.
    /// </summary>
    /// <param name="roleName"></param>
    /// <param name="claimRequests"></param>
    /// <returns></returns>
    public async Task<ApiResponse<object>> AddClaim(string roleName, List<ClaimRequest> claimRequests)
    {
        Log.Information($"[LOG INFORMATION] - SET TITLE {nameof(RoleService)} - METHOD {nameof(AddClaim)}\n");

        try
        {
            Log.Information($"[LOG INFORMATION] - Adicionando uma novas claims na role {roleName}\n");

            #region Add claim in exist role.
            // Get the role for Id.
            var role = await _roleManager.Roles.FirstOrDefaultAsync(role => roleName.Equals(role.Name));

            // Verify is nor null role.
            if (role is not null)
            {
                // foreach claims.
                foreach (var claim in claimRequests)
                {
                    // Add claims in role.
                    await _roleManager.AddClaimAsync(role, new Claim(claim.Type, claim.Value));

                    Log.Information($"[LOG INFORMATION] - Claim {claim.Type}/{claim.Value} adicionada.\n");
                }

                // Response success.
                return new ApiResponse<object>(true, StatusCodes.SuccessOK, new List<DadosNotificacao> { new DadosNotificacao($"Claim adicionada a role {roleName} com sucesso.") });
            }
            #endregion

            // Response error.
            return new ApiResponse<object>(false, StatusCodes.ErrorNotFound, new List<DadosNotificacao> { new DadosNotificacao($"Role com o nome {roleName} não existe.") });
        }
        catch (Exception exception)
        {
            Log.Error($"[LOG ERROR] - {exception.InnerException} - {exception.Message}\n");

            // Error response.
            return new ApiResponse<object>(false, StatusCodes.ServerErrorInternalServerError, new List<DadosNotificacao> { new DadosNotificacao(exception.Message) });
        }
    }

    /// <summary>
    /// Método responsável por remover uma claim na role.
    /// </summary>
    /// <param name="roleName"></param>
    /// <param name="claimRequests"></param>
    /// <returns></returns>
    public async Task<ApiResponse<object>> RemoveClaim(string roleName, ClaimRequest claimRequests)
    {
        Log.Information($"[LOG INFORMATION] - SET TITLE {nameof(RoleService)} - METHOD {nameof(RemoveClaim)}\n");

        try
        {
            Log.Information($"[LOG INFORMATION] - Removendo a claim {claimRequests.Type}/{claimRequests.Value} da Role {roleName}\n");

            #region Add claim in exist role.
            // Get role for Id.
            var role = await _roleManager.Roles.FirstOrDefaultAsync(role => roleName.Equals(role.Name));

            // Verify is not null role.
            if (role is not null)
            {
                // Remove remove claim.
                await _roleManager.RemoveClaimAsync(role, new Claim(claimRequests.Type, claimRequests.Value));

                // Response success.
               return new ApiResponse<object>(true, StatusCodes.SuccessOK, new List<DadosNotificacao> { new DadosNotificacao($"Claim removida da role {roleName} com sucesso.") });
            }
            #endregion

            return new ApiResponse<object>(false, StatusCodes.ErrorNotFound, new List<DadosNotificacao> { new DadosNotificacao($"Role com o nome {roleName} não existe.") });
        }
        catch (Exception exception)
        {
            Log.Error($"[LOG ERROR] - {exception.InnerException} - {exception.Message}\n");

            // Error response.
            return new ApiResponse<object>(false, StatusCodes.ServerErrorInternalServerError, new List<DadosNotificacao> { new DadosNotificacao(exception.Message) });
        }
    }
    #endregion
}