using APPLICATION.APPLICATION.CONFIGURATIONS;
using APPLICATION.DOMAIN.CONTRACTS.SERVICES.PERSON;
using APPLICATION.DOMAIN.CONTRACTS.SERVICES.TOKEN;
using APPLICATION.DOMAIN.CONTRACTS.SERVICES.USER;
using APPLICATION.DOMAIN.DTOS.CONFIGURATION;
using APPLICATION.DOMAIN.DTOS.REQUEST;
using APPLICATION.DOMAIN.DTOS.REQUEST.PEOPLE;
using APPLICATION.DOMAIN.DTOS.REQUEST.USER;
using APPLICATION.DOMAIN.DTOS.RESPONSE.UTILS;
using APPLICATION.DOMAIN.UTILS.Extensions;
using APPLICATION.DOMAIN.VALIDATORS;
using APPLICATION.INFRAESTRUTURE.FACADES.EMAIL;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
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

        private readonly IPersonService _personService;
        #endregion

        public UserService(SignInManager<IdentityUser<Guid>> signInManager, UserManager<IdentityUser<Guid>> userManager, IOptions<AppSettings> appsettings, EmailFacade emailFacade, ITokenService tokenService, IMapper mapper, IPersonService personService)
        {
            _signInManager = signInManager;

            _userManager = userManager;

            _appsettings = appsettings;

            _emailFacade = emailFacade;

            _tokenService = tokenService;

            _mapper = mapper;

            _personService = personService;
        }

        #region Authentication
        /// <summary>
        /// Método responsável por fazer a authorização do usuário.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ApiResponse<object>> Authentication(LoginRequest loginRequest)
        {
            Log.Information($"[LOG INFORMATION] - SET TITLE {nameof(UserService)} - METHOD {nameof(Authentication)}\n");

            try
            {
                Log.Information($"[LOG INFORMATION] - Validando request.\n");

                // Validate de userRequest.
                var validation = await new AuthenticationValidator().ValidateAsync(loginRequest); if (validation.IsValid is false) return validation.CarregarErrosValidator();

                Log.Information($"[LOG INFORMATION] - Request validado com sucesso.\n");

                Log.Information($"[LOG INFORMATION] - Recuperando usuário {JsonConvert.SerializeObject(loginRequest)}.\n");

                // sigin user wirh username & password
                var signInResult = await _signInManager.PasswordSignInAsync(loginRequest.Username, loginRequest.Password, true, true);
                              
                // return error response
                if (signInResult.Succeeded is false)
                {
                    // locked user
                    if (signInResult.IsLockedOut)
                    {
                        Log.Information($"[LOG INFORMATION] - Falha ao recuperar usuário, está bloqueado.\n");

                        return new ApiResponse<object>(signInResult.Succeeded, new List<DadosNotificacao> { new DadosNotificacao(DOMAIN.ENUM.StatusCodes.ErrorLocked, "Usuário está bloqueado. Caso não desbloqueie em alguns minutos entre em contato com o suporte.") });
                    }
                    else if (signInResult.IsNotAllowed) // not allowed user
                    {
                        Log.Information($"[LOG INFORMATION] - Falha ao recuperar usuário, não está confirmado.\n");

                        return new ApiResponse<object>(signInResult.Succeeded, new List<DadosNotificacao> { new DadosNotificacao(DOMAIN.ENUM.StatusCodes.ErrorUnauthorized, "Email do usuário não está confirmado.") });
                    }
                    else if (signInResult.RequiresTwoFactor) // requires two factor user
                    {
                        Log.Information($"[LOG INFORMATION] - Falha ao recuperar usuário, requer verificação de dois fatores.\n");

                        return new ApiResponse<object>(signInResult.Succeeded, new List<DadosNotificacao> { new DadosNotificacao(DOMAIN.ENUM.StatusCodes.ErrorUnauthorized, "Usuário necessita de verificação de dois fatores.") });
                    }
                    else // incorrects params user
                    {
                        Log.Information($"[LOG INFORMATION] - Falha ao recuperar usuário, dados incorretos.\n");

                        return new ApiResponse<object>(signInResult.Succeeded, new List<DadosNotificacao> { new DadosNotificacao(DOMAIN.ENUM.StatusCodes.ErrorUnauthorized, "Os dados do usuário estão inválidos ou usuário não existe.") });
                    }
                }

                Log.Information($"[LOG INFORMATION] - Usuário recuperado com sucesso.\n");

                Log.Information($"[LOG INFORMATION] - Gerando token.\n");

                // Create token.
                var token = await _tokenService.CreateJsonWebToken(loginRequest.Username);

                Log.Information($"[LOG INFORMATION] - Token gerado {token.Value}.\n");

                // return the token.
                return new ApiResponse<object>(signInResult.Succeeded, token);
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
                Log.Information($"[LOG INFORMATION] - Validando request.\n");

                #region validate requests
                var validation = await new CreatePersonValidator().ValidateAsync(personFastRequest); if (validation.IsValid is false) return validation.CarregarErrosValidator();
                #endregion

                Log.Information($"[LOG INFORMATION] - Request validado com sucesso.\n");

                #region Conver request to identity
                var identityUser = personFastRequest.User.ToIdentityUser();
                #endregion

                #region User create & set roles & claims
                var response = await BuildUser(identityUser, personFastRequest.User);
                #endregion

                if (response.Succeeded)
                {
                    #region Person create
                    await _personService.Create(personFastRequest, identityUser.Id);
                    #endregion

                    #region Invite e-mail confirmation
                    await ConfirmeUserForEmail(identityUser);
                    #endregion

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
        /// <param name="activateUserRequest"></param>
        /// <returns></returns>
        public async Task<ApiResponse<object>> Activate(ActivateUserRequest activateUserRequest)
        {
            Log.Information($"[LOG INFORMATION] - SET TITLE {nameof(UserService)} - METHOD {nameof(Activate)}\n");

            try
            {
                Log.Information($"[LOG INFORMATION] - Recuperando usuário.\n");

                var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == activateUserRequest.UserId);

                Log.Information($"[LOG INFORMATION] - Confirmando e-mail do usuário {user.UserName}.\n");

                var response = await _userManager.ConfirmEmailAsync(user, HttpUtility.UrlDecode(activateUserRequest.Code.Replace(";", "%")));

                if (response.Succeeded)
                {
                    Log.Information($"[LOG INFORMATION] - Usuário ativado com sucesso.\n");

                    return new ApiResponse<object>(response.Succeeded, new List<DadosNotificacao> { new DadosNotificacao(DOMAIN.ENUM.StatusCodes.SuccessOK, "Usuário ativado com sucesso.") });
                }

                Log.Information($"[LOG INFORMATION] - Falha na ativãção do usuário.\n");

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
                Log.Information($"[LOG INFORMATION] - Recuperando usuario {username}.\n");

                var user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName.Equals(username));

                Log.Information($"[LOG INFORMATION] - Adicionando a claim ({claimRequest.Type}/{claimRequest.Value}) no usuário.\n");

                #region User set claim
                var response = await _userManager.AddClaimAsync(user, new Claim(claimRequest.Type, claimRequest.Value));
                #endregion

                if (response.Succeeded)
                {
                    Log.Information($"[LOG INFORMATION] - Claim adicionada com sucesso.\n");

                    return new ApiResponse<object>(response.Succeeded, new List<DadosNotificacao> { new DadosNotificacao(DOMAIN.ENUM.StatusCodes.SuccessCreated, $"Claim {claimRequest.Type} / {claimRequest.Value}, adicionada com sucesso ao usuário {username}.") });
                }

                Log.Information($"[LOG INFORMATION] - Falha ao adicionar claim.\n");

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
                Log.Information($"[LOG INFORMATION] - Recuperando usuário {username}.\n");

                var user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName.Equals(username));

                Log.Information($"[LOG INFORMATION] - Removendo a claim ({claimRequest.Type}/{claimRequest.Value}) do usuário.\n");

                #region User remove claim
                var response = await _userManager.RemoveClaimAsync(user, new Claim(claimRequest.Type, claimRequest.Value));
                #endregion

                if (response.Succeeded)
                {
                    Log.Information($"[LOG INFORMATION] - Claim remvida com sucesso.\n");

                    return new ApiResponse<object>(response.Succeeded, new List<DadosNotificacao> { new DadosNotificacao(DOMAIN.ENUM.StatusCodes.SuccessCreated, $"Claim {claimRequest.Type} / {claimRequest.Value}, removida com sucesso do usuário {username}.") });
                }

                Log.Information($"[LOG INFORMATION] - Falha ao remover claim.\n");

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
                Log.Information($"[LOG INFORMATION] - Recuperando dados do usuário {username}.\n");

                var user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName.Equals(username));

                Log.Information($"[LOG INFORMATION] - Adicionando a role ({roleName}) ao usuário.\n");

                #region User set claim
                var response = await _userManager.AddToRoleAsync(user, roleName);
                #endregion

                if (response.Succeeded)
                {
                    Log.Information($"[LOG INFORMATION] - Role adicionada com sucesso.\n");

                    return new ApiResponse<object>(response.Succeeded, new List<DadosNotificacao> { new DadosNotificacao(DOMAIN.ENUM.StatusCodes.SuccessCreated, $"Role {roleName}, adicionada com sucesso ao usuário {username}.") });
                }

                Log.Information($"[LOG INFORMATION] - Falha ao adicionar role.\n");

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
                Log.Information($"[LOG INFORMATION] - Recuperando dados do usuário {username}.\n");

                var user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName.Equals(username));

                Log.Information($"[LOG INFORMATION] - Removendo role ({roleName}) do usuário.\n");

                #region User remove claim
                var response = await _userManager.RemoveFromRoleAsync(user, roleName);
                #endregion

                if (response.Succeeded)
                {
                    Log.Information($"[LOG INFORMATION] - Role removida com sucesso.\n");

                    return new ApiResponse<object>(response.Succeeded, new List<DadosNotificacao> { new DadosNotificacao(DOMAIN.ENUM.StatusCodes.SuccessCreated, $"Role {roleName}, removida com sucesso do usuário {username}.") });
                }

                Log.Information($"[LOG INFORMATION] - Falha ao remover role.\n");

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
                Log.Information("[LOG INFORMATION] - Criando usuário\n");

                // Create User.
                var identityResult =  await _userManager.CreateAsync(user, userRequest.Password);

                if(identityResult.Succeeded)
                {
                    Log.Information($"[LOG INFORMATION] - Usuário criado com sucesso {user.UserName}, dados {JsonConvert.SerializeObject(userRequest)}\n");

                    await _userManager.AddLoginAsync(user, new UserLoginInfo("TOOLS.USER.API", "TOOLS.USER", "TOOLS.USER.PROVIDER.KEY"));

                    return identityResult;
                }

                Log.Information($"[LOG INFORMATION] - Falha ao remover role.\n");

                return identityResult;
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
                Log.Information($"[LOG INFORMATION] - Gerando codigo de confirmação de e-mail.\n");

                var emailCode = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                var codifyEmailCode = HttpUtility.UrlEncode(emailCode).Replace("%", ";");

                Log.Information($"[LOG INFORMATION] - Código gerado - {codifyEmailCode}.\n");

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
