using APPLICATION.DOMAIN.CONTRACTS.SERVICES.USER;
using APPLICATION.DOMAIN.DTOS.REQUEST.USER;
using APPLICATION.DOMAIN.DTOS.RESPONSE.UTILS;
using APPLICATION.DOMAIN.ENTITY.ROLE;
using APPLICATION.DOMAIN.ENUM;
using APPLICATION.DOMAIN.UTILS.EXTENSIONS;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Security.Claims;

namespace APPLICATION.APPLICATION.SERVICES.USER;

public class RoleService : IRoleService
{
    private readonly RoleManager<RoleEntity> _roleManager;

    private readonly IMapper _autoMapper;

    public RoleService(RoleManager<RoleEntity> roleManager, IMapper autoMapper)
    {
        _roleManager = roleManager;

        _autoMapper = autoMapper;
    }

    #region Role
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
            var role = roleRequest.ToIdentityRole();

            Log.Information($"[LOG INFORMATION] - Criando nova Role {roleRequest.Name}\n");

            // Create a role in database.
            var response = await _roleManager.CreateAsync(role);

            // Is success enter.
            if (response.Succeeded)
            {
                Log.Information($"[LOG INFORMATION] - Adicionando claims na role {roleRequest.Name}\n");

                foreach (var claim in roleRequest.Claims)
                {
                    await _roleManager.AddClaimAsync(role, new Claim(claim.Type, claim.Value));
                }

                // Response success.
                return new ApiResponse<object>(response.Succeeded, StatusCodes.SuccessCreated, null, new List<DadosNotificacao> { new DadosNotificacao("Role criado com sucesso.") });
            }
            #endregion

            Log.Information($"[LOG INFORMATION] - Falha ao criar role.\n");

            // Response error.
            return new ApiResponse<object>(response.Succeeded, StatusCodes.ErrorBadRequest, null, response.Errors.Select(e => new DadosNotificacao(e.Description)).ToList());
        }
        catch (Exception exception)
        {
            Log.Error($"[LOG ERROR] - {exception.InnerException} - {exception.Message}\n");

            // Error response.
            return new ApiResponse<object>(false, StatusCodes.ServerErrorInternalServerError, null, new List<DadosNotificacao> { new DadosNotificacao(exception.Message) });
        }
    }
    #endregion

    #region Claim
    /// <summary>
    /// Método responsavel por adicionar uma claim na role.
    /// </summary>
    /// <param name="roleName"></param>
    /// <param name="claimRequests"></param>
    /// <returns></returns>
    public async Task<ApiResponse<object>> AddClaims(RoleRequest roleRequest)
    {
        Log.Information($"[LOG INFORMATION] - SET TITLE {nameof(RoleService)} - METHOD {nameof(AddClaims)}\n");

        try
        {
            Log.Information($"[LOG INFORMATION] - Adicionando uma novas claims na role {roleRequest.Name}\n");

            #region Add claim in exist role.
            // Get the role for Id.
            var role = await _roleManager.Roles.FirstOrDefaultAsync(role => roleRequest.Name.Equals(role.Name) && role.CompanyId.Equals(roleRequest.CompanyId));

            // Verify is nor null role.
            if (role is not null)
            {
                // foreach claims.
                foreach (var claim in roleRequest.Claims)
                {
                    // Add claims in role.
                    await _roleManager.AddClaimAsync(role, new Claim(claim.Type, claim.Value));

                    Log.Information($"[LOG INFORMATION] - Claim {claim.Type}/{claim.Value} adicionada.\n");
                }

                // Response success.
                return new ApiResponse<object>(true, StatusCodes.SuccessOK, null, new List<DadosNotificacao> { new DadosNotificacao($"Claim adicionada a role {roleRequest.Name} com sucesso.") });
            }
            #endregion

            // Response error.
            return new ApiResponse<object>(false, StatusCodes.ErrorNotFound, null, new List<DadosNotificacao> { new DadosNotificacao($"Role com o nome {roleRequest.Name} não existe.") });
        }
        catch (Exception exception)
        {
            Log.Error($"[LOG ERROR] - {exception.InnerException} - {exception.Message}\n");

            // Error response.
            return new ApiResponse<object>(false, StatusCodes.ServerErrorInternalServerError, null, new List<DadosNotificacao> { new DadosNotificacao(exception.Message) });
        }
    }

    /// <summary>
    /// Método responsável por remover uma claim na role.
    /// </summary>
    /// <param name="roleName"></param>
    /// <param name="claimRequests"></param>
    /// <returns></returns>
    public async Task<ApiResponse<object>> RemoveClaims(RoleRequest roleRequest)
    {
        Log.Information($"[LOG INFORMATION] - SET TITLE {nameof(RoleService)} - METHOD {nameof(RemoveClaims)}\n");

        try
        {
            Log.Information($"[LOG INFORMATION] - Removendo as claims da Role {roleRequest.Name}\n");
            
            // Get role for Id.
            var role = await _roleManager.Roles.FirstOrDefaultAsync(role => roleRequest.Name.Equals(role.Name) && role.CompanyId.Equals(roleRequest.CompanyId));

            // Verify is not null role.
            if (role is not null)
            {
                // Remove remove claim.
                foreach(var claim in roleRequest.Claims)
                {
                    await _roleManager.RemoveClaimAsync(role, new Claim(claim.Type, claim.Value));

                    Log.Information($"[LOG INFORMATION] - Claim {claim.Type} / {claim.Value} removida com sucesso.\n");
                }

                // Response success.
                return new ApiResponse<object>(true, StatusCodes.SuccessOK, null, new List<DadosNotificacao> { new DadosNotificacao($"Claim removida da role {roleRequest.Name} com sucesso.") });
            }

            Log.Information($"[LOG INFORMATION] - Role não existe.\n");

            return new ApiResponse<object>(false, StatusCodes.ErrorNotFound, null, new List<DadosNotificacao> { new DadosNotificacao($"Role com o nome {roleRequest.Name} não existe.") });
        }
        catch (Exception exception)
        {
            Log.Error($"[LOG ERROR] - {exception.Message}\n");

            // Error response.
            return new ApiResponse<object>(false, StatusCodes.ServerErrorInternalServerError, null, new List<DadosNotificacao> { new DadosNotificacao(exception.Message) });
        }
    }
    #endregion
}