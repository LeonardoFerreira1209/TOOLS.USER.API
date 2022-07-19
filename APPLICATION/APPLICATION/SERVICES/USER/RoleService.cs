using APPLICATION.DOMAIN.CONTRACTS.SERVICES.USER;
using APPLICATION.DOMAIN.DTOS.REQUEST.USER;
using APPLICATION.DOMAIN.DTOS.RESPONSE;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
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
            var role = _autoMapper.Map<IdentityRole<Guid>>(roleRequest);

            var response = await _roleManager.CreateAsync(role);

            if (response.Succeeded)
            {
                roleRequest.Claims.ForEach(claim => _roleManager.AddClaimAsync(role, new Claim(claim.Type, claim.Value)));

                return new ApiResponse<object>(response.Succeeded, new List<DadosNotificacao> { new DadosNotificacao(DOMAIN.ENUM.StatusCodes.SuccessCreated, "Role criado com sucesso.") });
            }
            #endregion

            return new ApiResponse<object>(response.Succeeded, response.Errors.Select(e => new DadosNotificacao(DOMAIN.ENUM.StatusCodes.ErrorBadRequest, e.Description)).ToList());
        }
        catch (Exception exception)
        {
            Log.Error("[LOG ERROR]", exception, exception.Message);

            return new ApiResponse<object>(false, new List<DadosNotificacao> { new DadosNotificacao(DOMAIN.ENUM.StatusCodes.ServerErrorInternalServerError, exception.Message) });
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
        Log.Information($"[LOG INFORMATION] - SET TITLE {nameof(RoleService)} - METHOD {nameof(Create)}\n");

        try
        {
            #region Add claim in exist role.
            var role = await _roleManager.Roles.FirstOrDefaultAsync(role => roleName.Equals(role.Name));

            if (role is not null)
            {
                foreach (var claim in claimRequests)
                {
                    await _roleManager.AddClaimAsync(role, new Claim(claim.Type, claim.Value));
                }

                return new ApiResponse<object>(true, new List<DadosNotificacao> { new DadosNotificacao(DOMAIN.ENUM.StatusCodes.SuccessCreated, $"Claim adicionada a role {roleName} com sucesso.") });
            }
            #endregion

            return new ApiResponse<object>(false, new List<DadosNotificacao> { new DadosNotificacao(DOMAIN.ENUM.StatusCodes.ErrorBadRequest, $"Role com o nome {roleName} não existe.") });
        }
        catch (Exception exception)
        {
            Log.Error("[LOG ERROR]", exception, exception.Message);

            return new ApiResponse<object>(false, new List<DadosNotificacao> { new DadosNotificacao(DOMAIN.ENUM.StatusCodes.ServerErrorInternalServerError, exception.Message) });
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
        Log.Information($"[LOG INFORMATION] - SET TITLE {nameof(RoleService)} - METHOD {nameof(Create)}\n");

        try
        {
            #region Add claim in exist role.
            var role = await _roleManager.Roles.FirstOrDefaultAsync(role => roleName.Equals(role.Name));

            if (role is not null)
            {
                await _roleManager.RemoveClaimAsync(role, new Claim(claimRequests.Type, claimRequests.Value));

                return new ApiResponse<object>(true, new List<DadosNotificacao> { new DadosNotificacao(DOMAIN.ENUM.StatusCodes.SuccessCreated, $"Claim removida da role {roleName} com sucesso.") });
            }
            #endregion

            return new ApiResponse<object>(false, new List<DadosNotificacao> { new DadosNotificacao(DOMAIN.ENUM.StatusCodes.ErrorBadRequest, $"Role com o nome {roleName} não existe.") });
        }
        catch (Exception exception)
        {
            Log.Error("[LOG ERROR]", exception, exception.Message);

            return new ApiResponse<object>(false, new List<DadosNotificacao> { new DadosNotificacao(DOMAIN.ENUM.StatusCodes.ServerErrorInternalServerError, exception.Message) });
        }
    }
    #endregion
}
