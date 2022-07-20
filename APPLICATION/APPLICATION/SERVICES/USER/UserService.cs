using APPLICATION.APPLICATION.CONFIGURATIONS;
using APPLICATION.DOMAIN.CONTRACTS.SERVICES.TOKEN;
using APPLICATION.DOMAIN.CONTRACTS.SERVICES.USER;
using APPLICATION.DOMAIN.DTOS.CONFIGURATION;
using APPLICATION.DOMAIN.DTOS.REQUEST;
using APPLICATION.DOMAIN.DTOS.REQUEST.PEOPLE;
using APPLICATION.DOMAIN.DTOS.REQUEST.USER;
using APPLICATION.DOMAIN.DTOS.RESPONSE;
using APPLICATION.DOMAIN.VALIDATORS;
using APPLICATION.INFRAESTRUTURE.FACADES.EMAIL;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Serilog;
using System.Security.Claims;
using System.Web;

namespace APPLICATION.APPLICATION.SERVICES.USER
{
    /// <summary>
    /// Serviço de usuários.
    /// </summary>
    public class UserService : IUserService
    {
        #region privates
        private readonly SignInManager<IdentityUser<Guid>> _signInManager;

        private readonly UserManager<IdentityUser<Guid>> _userManager;

        private readonly IOptions<AppSettings> _appsettings;

        private readonly EmailFacade _emailFacade;

        private readonly ITokenService _tokenService;

        private readonly IMapper _mapper;
        #endregion

        public UserService(SignInManager<IdentityUser<Guid>> signInManager, UserManager<IdentityUser<Guid>> userManager, IOptions<AppSettings> appsettings, EmailFacade emailFacade, ITokenService tokenService, IMapper mapper)
        {
            _signInManager = signInManager;

            _userManager = userManager;

            _appsettings = appsettings;

            _emailFacade = emailFacade;

            _tokenService = tokenService;

            _mapper = mapper;
        }

        #region Authentication
        /// <summary>
        /// Método responsável por fazer a authorização do usuário.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ApiResponse<object>> Authentication(LoginRequest userRequest)
        {
            Log.Information($"[LOG INFORMATION] - SET TITLE {nameof(UserService)} - METHOD {nameof(Authentication)}\n");

            try
            {
                var validation = await new AuthenticationValidator().ValidateAsync(userRequest);

                if (validation.IsValid is false) return validation.CarregarErrosValidator();

                var signInResult = await _signInManager.PasswordSignInAsync(userRequest.Username, userRequest.Password, true, true);

                if (signInResult.Succeeded is false)
                {
                    if (signInResult.IsLockedOut)
                    {
                        return new ApiResponse<object>(signInResult.Succeeded, new List<DadosNotificacao> { new DadosNotificacao(DOMAIN.ENUM.StatusCodes.ErrorLocked, "Usuário está bloqueado. Caso não desbloqueie em alguns minutos entre em contato com o suporte.") });
                    }
                    else if (signInResult.IsNotAllowed)
                    {
                        return new ApiResponse<object>(signInResult.Succeeded, new List<DadosNotificacao> { new DadosNotificacao(DOMAIN.ENUM.StatusCodes.ErrorUnauthorized, "Email do usuário não está confirmado.") });
                    }
                    else if (signInResult.RequiresTwoFactor)
                    {
                        return new ApiResponse<object>(signInResult.Succeeded, new List<DadosNotificacao> { new DadosNotificacao(DOMAIN.ENUM.StatusCodes.ErrorUnauthorized, "Usuário necessita de verificação de dois fatores.") });
                    }
                    else
                    {
                        return new ApiResponse<object>(signInResult.Succeeded, new List<DadosNotificacao> { new DadosNotificacao(DOMAIN.ENUM.StatusCodes.ErrorUnauthorized, "Os dados do usuário estão inválidos ou usuário não existe.") });
                    }
                }

                return new ApiResponse<object>(signInResult.Succeeded, await _tokenService.CreateJsonWebToken(userRequest.Username));
            }
            catch (Exception exception)
            {
                Log.Error("[LOG ERROR]", exception, exception.Message);

                return new ApiResponse<object>(false, new List<DadosNotificacao> { new DadosNotificacao(DOMAIN.ENUM.StatusCodes.ServerErrorInternalServerError, exception.Message) });
            }
        }
        #endregion

        #region Create
        /// <summary>
        /// Método responsavel por criar um novo usuário.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ApiResponse<object>> Create(PersonFastRequest personFastRequest)
        {
            Log.Information($"[LOG INFORMATION] - SET TITLE {nameof(UserService)} - METHOD {nameof(Create)}\n");

            try
            {
                var validation = await new CreateUserValidator().ValidateAsync(personFastRequest.User);

                if (validation.IsValid is false) return validation.CarregarErrosValidator();

                var identityUser = _mapper.Map<IdentityUser<Guid>>(personFastRequest.User);

                #region User create & set roles & claims
                var response = await BuildUser(identityUser, personFastRequest.User);
                #endregion

                if (response.Succeeded)
                {
                    await ConfirmeUserForEmail(identityUser);

                    return new ApiResponse<object>(response.Succeeded, new List<DadosNotificacao> { new DadosNotificacao(DOMAIN.ENUM.StatusCodes.SuccessCreated, "Usuário criado com sucesso.") });
                }

                return new ApiResponse<object>(response.Succeeded, response.Errors.Select(e => new DadosNotificacao(DOMAIN.ENUM.StatusCodes.ErrorBadRequest, e.Description)).ToList());
            }
            catch (Exception exception)
            {
                Log.Error("[LOG ERROR]", exception, exception.Message);

                return new ApiResponse<object>(false, new List<DadosNotificacao> { new DadosNotificacao(DOMAIN.ENUM.StatusCodes.ServerErrorInternalServerError, exception.Message) });
            }
        }
        #endregion

        #region Activate
        /// <summary>
        /// Método responsavel por ativar um novo usuário.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ApiResponse<object>> Activate(ActivateUserRequest request)
        {
            Log.Information($"[LOG INFORMATION] - SET TITLE {nameof(UserService)} - METHOD {nameof(Activate)}\n");

            try
            {
                var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == request.UsuarioId);

                var response = await _userManager.ConfirmEmailAsync(user, HttpUtility.UrlDecode(request.Codigo.Replace(";", "%")));

                if (response.Succeeded) return new ApiResponse<object>(response.Succeeded, new List<DadosNotificacao> { new DadosNotificacao(DOMAIN.ENUM.StatusCodes.SuccessOK, "Usuário ativado com sucesso.") });

                return new ApiResponse<object>(response.Succeeded, new List<DadosNotificacao> { new DadosNotificacao(DOMAIN.ENUM.StatusCodes.ErrorBadRequest, "Falha ao ativar usuário.") });
            }
            catch (Exception exception)
            {
                Log.Error("[LOG ERROR]", exception, exception.Message);

                return new ApiResponse<object>(false, new List<DadosNotificacao> { new DadosNotificacao(DOMAIN.ENUM.StatusCodes.ServerErrorInternalServerError, exception.Message) });
            }
        }
        #endregion

        #region Roles & Claims
        /// <summary>
        /// Método responsavel por criar uma nova claim para o usuário.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ApiResponse<object>> AddClaim(string username, ClaimRequest claimRequest)
        {
            Log.Information($"[LOG INFORMATION] - SET TITLE {nameof(UserService)} - METHOD {nameof(AddClaim)}\n");

            try
            {
                var user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName.Equals(username));

                #region User set claim
                var response = await _userManager.AddClaimAsync(user, new Claim(claimRequest.Type, claimRequest.Value));
                #endregion

                if (response.Succeeded) return new ApiResponse<object>(response.Succeeded, new List<DadosNotificacao> { new DadosNotificacao(DOMAIN.ENUM.StatusCodes.SuccessCreated, $"Claim {claimRequest.Type} / {claimRequest.Value}, adicionada com sucesso ao usuário {username}.") });

                return new ApiResponse<object>(response.Succeeded, response.Errors.Select(e => new DadosNotificacao(DOMAIN.ENUM.StatusCodes.ErrorBadRequest, e.Description)).ToList());
            }
            catch (Exception exception)
            {
                Log.Error("[LOG ERROR]\n", exception, exception.Message);

                return new ApiResponse<object>(false, new List<DadosNotificacao> { new DadosNotificacao(DOMAIN.ENUM.StatusCodes.ServerErrorInternalServerError, exception.Message) });
            }
        }

        /// <summary>
        /// Método responsavel por remover uma claim do usuário.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="claimRequest"></param>
        /// <returns></returns>
        public async Task<ApiResponse<object>> RemoveClaim(string username, ClaimRequest claimRequest)
        {
            Log.Information($"[LOG INFORMATION] - SET TITLE {nameof(UserService)} - METHOD {nameof(RemoveClaim)}\n");

            try
            {
                var user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName.Equals(username));

                #region User remove claim
                var response = await _userManager.RemoveClaimAsync(user, new Claim(claimRequest.Type, claimRequest.Value));
                #endregion

                if (response.Succeeded) return new ApiResponse<object>(response.Succeeded, new List<DadosNotificacao> { new DadosNotificacao(DOMAIN.ENUM.StatusCodes.SuccessCreated, $"Claim {claimRequest.Type} / {claimRequest.Value}, removida com sucesso do usuário {username}.") });

                return new ApiResponse<object>(response.Succeeded, response.Errors.Select(e => new DadosNotificacao(DOMAIN.ENUM.StatusCodes.ErrorBadRequest, e.Description)).ToList());
            }
            catch (Exception exception)
            {
                Log.Error("[LOG ERROR]\n", exception, exception.Message);

                return new ApiResponse<object>(false, new List<DadosNotificacao> { new DadosNotificacao(DOMAIN.ENUM.StatusCodes.ServerErrorInternalServerError, exception.Message) });
            }
        }

        /// <summary>
        /// Método responsavel por adicionar uma role ao usuário.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public async Task<ApiResponse<object>> AddRole(string username, string roleName)
        {
            Log.Information($"[LOG INFORMATION] - SET TITLE {nameof(UserService)} - METHOD {nameof(AddRole)}\n");

            try
            {
                var user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName.Equals(username));

                #region User set claim
                var response = await _userManager.AddToRoleAsync(user, roleName);
                #endregion

                if (response.Succeeded) return new ApiResponse<object>(response.Succeeded, new List<DadosNotificacao> { new DadosNotificacao(DOMAIN.ENUM.StatusCodes.SuccessCreated, $"Role {roleName}, adicionada com sucesso ao usuário {username}.") });

                return new ApiResponse<object>(response.Succeeded, response.Errors.Select(e => new DadosNotificacao(DOMAIN.ENUM.StatusCodes.ErrorBadRequest, e.Description)).ToList());
            }
            catch (Exception exception)
            {
                Log.Error("[LOG ERROR]\n", exception, exception.Message);

                return new ApiResponse<object>(false, new List<DadosNotificacao> { new DadosNotificacao(DOMAIN.ENUM.StatusCodes.ServerErrorInternalServerError, exception.Message) });
            }
        }

        /// <summary>
        /// Método responsavel por remover uma role ao usuário.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="claimRequest"></param>
        /// <returns></returns>
        public async Task<ApiResponse<object>> RemoveRole(string username, string roleName)
        {
            Log.Information($"[LOG INFORMATION] - SET TITLE {nameof(UserService)} - METHOD {nameof(RemoveRole)}\n");

            try
            {
                var user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName.Equals(username));

                #region User remove claim
                var response = await _userManager.RemoveFromRoleAsync(user, roleName);
                #endregion

                if (response.Succeeded) return new ApiResponse<object>(response.Succeeded, new List<DadosNotificacao> { new DadosNotificacao(DOMAIN.ENUM.StatusCodes.SuccessCreated, $"Role {roleName}, removida com sucesso do usuário {username}.") });

                return new ApiResponse<object>(response.Succeeded, response.Errors.Select(e => new DadosNotificacao(DOMAIN.ENUM.StatusCodes.ErrorBadRequest, e.Description)).ToList());
            }
            catch (Exception exception)
            {
                Log.Error("[LOG ERROR]\n", exception, exception.Message);

                return new ApiResponse<object>(false, new List<DadosNotificacao> { new DadosNotificacao(DOMAIN.ENUM.StatusCodes.ServerErrorInternalServerError, exception.Message) });
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Método responsavel por gerar um usuário e vincular roles e claims.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="userRequest"></param>
        /// <returns></returns>
        private async Task<IdentityResult> BuildUser(IdentityUser<Guid> user, UserRequest userRequest)
        {
            Log.Information($"[LOG INFORMATION] - SET TITLE {nameof(UserService)} - METHOD {nameof(BuildUser)}\n");

            try
            {
                // Create User.
                var result = await _userManager.CreateAsync(user, userRequest.Password);

                userRequest.Claims.Add(new ClaimRequest("Id", user.Id.ToString()));

                // Add Roles & Claims to user.
                if (result.Succeeded)
                {
                    // Verify insert claims and roles.
                    var insertRoles = userRequest.RolesToUser is not null; var insertClaims = userRequest.Claims is not null;

                    // Insert roles to user.
                    if (insertRoles) await _userManager.AddToRolesAsync(user, userRequest.RolesToUser.SelectMany(r => r.Name));

                    // Insert claims to user.
                    if (insertClaims) await _userManager.AddClaimsAsync(user, userRequest.Claims.Select(c => new Claim(c.Type, c.Value)));

                    // Add login info to user.
                    await _userManager.AddLoginAsync(user, new UserLoginInfo("TOOLS.USER.API", "LOCAL", "@TOOLS.USER.API.PROJECT"));
                }

                return result;
            }
            catch (Exception exception)
            {
                Log.Error("[LOG ERROR]", exception, exception.Message);

                throw new Exception(exception.Message);
            }
        }

        /// <summary>
        /// Método responsavel por gerar um token de autorização e enviar por e-mail.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private async Task ConfirmeUserForEmail(IdentityUser<Guid> user)
        {
            Log.Information($"[LOG INFORMATION] - SET TITLE {nameof(UserService)} - METHOD {nameof(ConfirmeUserForEmail)}\n");

            try
            {
                var emailCode = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                var codifyEmailCode = HttpUtility.UrlEncode(emailCode).Replace("%", ";");

                await _emailFacade.Invite(new MailRequest
                {

                    Receivers = new List<string> { user.Email },
                    Link = $"{_appsettings.Value.UrlBase.BASE_URL}/security/activate/{codifyEmailCode}/{user.Id}",
                    Subject = "Ativação de e-mail",
                    Content = $"Olá {user.UserName}, estamos muito felizes com o seu cadastro em nosso sistema. Clique no botão para liberarmos o seu acesso.",
                    ButtonText = "Clique para ativar o e-mail",
                    TemplateName = "Welcome.Template"

                });
            }
            catch (Exception exception)
            {
                Log.Error("[LOG ERROR]", exception, exception.Message);

                throw new Exception(exception.Message);
            }
        }
        #endregion
    }
}
