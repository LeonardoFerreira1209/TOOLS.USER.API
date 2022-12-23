using APPLICATION.DOMAIN.CONTRACTS.SERVICES.USER;
using APPLICATION.DOMAIN.DTOS.REQUEST.USER;
using APPLICATION.DOMAIN.DTOS.RESPONSE.UTILS;
using APPLICATION.DOMAIN.ENTITY.ROLE;
using APPLICATION.DOMAIN.ENUM;
using APPLICATION.DOMAIN.UTILS.EXTENSIONS;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Security.Claims;

namespace APPLICATION.APPLICATION.SERVICES.USER;

public class RoleService : IRoleService
{
    private readonly RoleManager<RoleEntity> _roleManager;

    public RoleService(RoleManager<RoleEntity> roleManager)
    {
        _roleManager = roleManager;
    }

    /// <summary>
    /// Método responsavel por criar uma nova role.
    /// </summary>
    /// <param name="roleRequest"></param>
    /// <returns></returns>
    public async Task<ApiResponse<object>> CreateAsync(RoleRequest roleRequest)
    {
        Log.Information($"[LOG INFORMATION] - SET TITLE {nameof(RoleService)} - METHOD {nameof(CreateAsync)}\n");

        try
        {
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

    /// <summary>
    /// Método responsavel por adicionar uma claim na role.
    /// </summary>
    /// <param name="roleRequest"></param>
    /// <returns></returns>
    public async Task<ApiResponse<object>> AddClaimsAsync(RoleRequest roleRequest)
    {
        Log.Information($"[LOG INFORMATION] - SET TITLE {nameof(RoleService)} - METHOD {nameof(AddClaimsAsync)}\n");

        try
        {
            Log.Information($"[LOG INFORMATION] - Adicionando uma novas claims na role {roleRequest.Name}\n");

            // Get the role for Id.
            var role = await _roleManager.Roles.FirstOrDefaultAsync(role => roleRequest.Name.Equals(role.Name));

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
    /// <param name="roleRequest"></param>
    /// <returns></returns>
    public async Task<ApiResponse<object>> RemoveClaimsAsync(RoleRequest roleRequest)
    {
        Log.Information($"[LOG INFORMATION] - SET TITLE {nameof(RoleService)} - METHOD {nameof(RemoveClaimsAsync)}\n");

        try
        {
            Log.Information($"[LOG INFORMATION] - Removendo as claims da Role {roleRequest.Name}\n");
            
            // Get role for Id.
            var role = await _roleManager.Roles.FirstOrDefaultAsync(role => roleRequest.Name.Equals(role.Name));

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
}