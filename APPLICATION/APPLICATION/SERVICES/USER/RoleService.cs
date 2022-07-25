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
    public async Task<ObjectResult> Create(RoleRequest roleRequest)
    {
        Log.Information($"[LOG INFORMATION] - SET TITLE {nameof(RoleService)} - METHOD {nameof(Create)}\n");

        try
        {
            #region User create & set roles & claims
            // Mapper to entity.
            var role = _autoMapper.Map<IdentityRole<Guid>>(roleRequest);

            // Create a role in database.
            var response = await _roleManager.CreateAsync(role);

            // Is success enter.
            if (response.Succeeded)
            {
                // foreach and add claims in request
                roleRequest.Claims.ForEach(claim => _roleManager.AddClaimAsync(role, new Claim(claim.Type, claim.Value)));

                // Response success.
                var apiResponseSuccess = new ApiResponse<object>(response.Succeeded, new List<DadosNotificacao> { new DadosNotificacao(DOMAIN.ENUM.StatusCodes.SuccessCreated, "Role criado com sucesso.") });

                // Return response.
                return new ObjectResult(apiResponseSuccess) { StatusCode = (int)StatusCodes.SuccessCreated };
            }
            #endregion

            // Response error.
            var apiResponseError = new ApiResponse<object>(response.Succeeded, response.Errors.Select(e => new DadosNotificacao(DOMAIN.ENUM.StatusCodes.ErrorBadRequest, e.Description)).ToList());

            // Return response error.
            return new ObjectResult(apiResponseError) { StatusCode = (int)StatusCodes.ErrorBadRequest };
        }
        catch (Exception exception)
        {
            Log.Error($"[LOG ERROR] - {exception.InnerException} - {exception.Message}\n");

            // Error response.
            var apiResponseError = new ApiResponse<object>(false, new List<DadosNotificacao> { new DadosNotificacao(StatusCodes.ServerErrorInternalServerError, exception.Message) });

            // Return error.
            return new ObjectResult(apiResponseError) { StatusCode = (int)StatusCodes.ServerErrorInternalServerError };
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
    public async Task<ObjectResult> AddClaim(string roleName, List<ClaimRequest> claimRequests)
    {
        Log.Information($"[LOG INFORMATION] - SET TITLE {nameof(RoleService)} - METHOD {nameof(Create)}\n");

        try
        {
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
                }

                // Response success.
                var apiResponseSuccess = new ApiResponse<object>(true, new List<DadosNotificacao> { new DadosNotificacao(DOMAIN.ENUM.StatusCodes.SuccessOK, $"Claim adicionada a role {roleName} com sucesso.") });

                // Return success
                return new ObjectResult(apiResponseSuccess) { StatusCode = (int)StatusCodes.SuccessOK };
            }
            #endregion

            // Response error.
            var apiResponseError = new ApiResponse<object>(false, new List<DadosNotificacao> { new DadosNotificacao(DOMAIN.ENUM.StatusCodes.ErrorNotFound, $"Role com o nome {roleName} não existe.") });

            // Return response
            return new ObjectResult(apiResponseError) { StatusCode = (int)StatusCodes.ErrorNotFound };
        }
        catch (Exception exception)
        {
            Log.Error($"[LOG ERROR] - {exception.InnerException} - {exception.Message}\n");

            // Error response.
            var apiResponseError = new ApiResponse<object>(false, new List<DadosNotificacao> { new DadosNotificacao(StatusCodes.ServerErrorInternalServerError, exception.Message) });

            // Return error.
            return new ObjectResult(apiResponseError) { StatusCode = (int)StatusCodes.ServerErrorInternalServerError };
        }
    }

    /// <summary>
    /// Método responsável por remover uma claim na role.
    /// </summary>
    /// <param name="roleName"></param>
    /// <param name="claimRequests"></param>
    /// <returns></returns>
    public async Task<ObjectResult> RemoveClaim(string roleName, ClaimRequest claimRequests)
    {
        Log.Information($"[LOG INFORMATION] - SET TITLE {nameof(RoleService)} - METHOD {nameof(Create)}\n");

        try
        {
            #region Add claim in exist role.
            // Get role for Id.
            var role = await _roleManager.Roles.FirstOrDefaultAsync(role => roleName.Equals(role.Name));

            // Verify is not null role.
            if (role is not null)
            {
                // Remove remove claim.
                await _roleManager.RemoveClaimAsync(role, new Claim(claimRequests.Type, claimRequests.Value));

                // Response success.
                var apiResponseSuccess = new ApiResponse<object>(true, new List<DadosNotificacao> { new DadosNotificacao(DOMAIN.ENUM.StatusCodes.SuccessOK, $"Claim removida da role {roleName} com sucesso.") });

                return new ObjectResult(apiResponseSuccess) { StatusCode = (int)StatusCodes.SuccessOK };
            }
            #endregion

            var apiResponseError = new ApiResponse<object>(false, new List<DadosNotificacao> { new DadosNotificacao(DOMAIN.ENUM.StatusCodes.ErrorNotFound, $"Role com o nome {roleName} não existe.") });

            return new ObjectResult(apiResponseError) { StatusCode = (int)StatusCodes.ErrorNotFound };
        }
        catch (Exception exception)
        {
            Log.Error($"[LOG ERROR] - {exception.InnerException} - {exception.Message}\n");

            // Error response.
            var apiResponseError = new ApiResponse<object>(false, new List<DadosNotificacao> { new DadosNotificacao(StatusCodes.ServerErrorInternalServerError, exception.Message) });

            // Return error.
            return new ObjectResult(apiResponseError) { StatusCode = (int)StatusCodes.ServerErrorInternalServerError };
        }
    }
    #endregion
}