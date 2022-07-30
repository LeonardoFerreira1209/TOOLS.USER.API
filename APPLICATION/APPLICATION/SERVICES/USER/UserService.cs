using APPLICATION.APPLICATION.CONFIGURATIONS;
using APPLICATION.DOMAIN.CONTRACTS.SERVICES.PERSON;
using APPLICATION.DOMAIN.CONTRACTS.SERVICES.TOKEN;
using APPLICATION.DOMAIN.CONTRACTS.SERVICES.USER;
using APPLICATION.DOMAIN.DTOS.CONFIGURATION;
using APPLICATION.DOMAIN.DTOS.REQUEST;
using APPLICATION.DOMAIN.DTOS.REQUEST.PEOPLE;
using APPLICATION.DOMAIN.DTOS.REQUEST.USER;
using APPLICATION.DOMAIN.DTOS.RESPONSE.UTILS;
using APPLICATION.DOMAIN.ENUM;
using APPLICATION.DOMAIN.UTILS.Extensions;
using APPLICATION.DOMAIN.VALIDATORS;
using APPLICATION.INFRAESTRUTURE.FACADES.EMAIL;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<ObjectResult> Authentication(LoginRequest loginRequest)
        {
            Log.Information($"[LOG INFORMATION] - SET TITLE {nameof(UserService)} - METHOD {nameof(Authentication)}\n");

            try
            {
                Log.Information($"[LOG INFORMATION] - Validando request.\n");

                // Validate de userRequest.
                var validation = await new AuthenticationValidator().ValidateAsync(loginRequest); if (validation.IsValid is false) return validation.CarregarErrosValidator();

                Log.Information($"[LOG INFORMATION] - Request validado com sucesso.\n");

                Log.Information($"[LOG INFORMATION] - Recuperando usuário {JsonConvert.SerializeObject(loginRequest)}.\n");

                // sigin user wirh username & password.
                var signInResult = await _signInManager.PasswordSignInAsync(loginRequest.Username, loginRequest.Password, true, true);

                // return error response.
                if (signInResult.Succeeded is false)
                {
                    // locked user.
                    if (signInResult.IsLockedOut)
                    {
                        Log.Information($"[LOG INFORMATION] - Falha ao recuperar usuário, está bloqueado.\n");

                        // Response Locked.
                        var apiResponseErrorLocked = new ApiResponse<object>(signInResult.Succeeded, new List<DadosNotificacao> { new DadosNotificacao(StatusCodes.ErrorLocked, "Usuário está bloqueado. Caso não desbloqueie em alguns minutos entre em contato com o suporte.") });

                        // Return error response.
                        return new ObjectResult(apiResponseErrorLocked) { StatusCode = (int)StatusCodes.ErrorLocked };
                    }
                    else if (signInResult.IsNotAllowed) // not allowed user.
                    {
                        Log.Information($"[LOG INFORMATION] - Falha ao recuperar usuário, não está confirmado.\n");

                        // Response notAllowed.
                        var apiResponseErrorNotAllowed = new ApiResponse<object>(signInResult.Succeeded, new List<DadosNotificacao> { new DadosNotificacao(StatusCodes.ErrorUnauthorized, "Email do usuário não está confirmado.") });

                        // Return error response.
                        return new ObjectResult(apiResponseErrorNotAllowed) { StatusCode = (int)StatusCodes.ErrorUnauthorized };
                    }
                    else if (signInResult.RequiresTwoFactor) // requires two factor user.
                    {
                        Log.Information($"[LOG INFORMATION] - Falha ao recuperar usuário, requer verificação de dois fatores.\n");

                        // Response twoFactor.
                        var apiResponseErrorTwoFactor = new ApiResponse<object>(signInResult.Succeeded, new List<DadosNotificacao> { new DadosNotificacao(StatusCodes.ErrorUnauthorized, "Usuário necessita de verificação de dois fatores.") });

                        // Return error response.
                        return new ObjectResult(apiResponseErrorTwoFactor) { StatusCode = (int)StatusCodes.ErrorUnauthorized };
                    }
                    else // incorrects params user.
                    {
                        Log.Information($"[LOG INFORMATION] - Falha ao recuperar usuário, dados incorretos.\n");

                        // Response error unathorized.
                        var apiResponseErrorUnauthorized = new ApiResponse<object>(signInResult.Succeeded, new List<DadosNotificacao> { new DadosNotificacao(StatusCodes.ErrorUnauthorized, "Os dados do usuário estão inválidos ou usuário não existe.") });

                        // Return error response.
                        return new ObjectResult(apiResponseErrorUnauthorized) { StatusCode = (int)StatusCodes.ErrorUnauthorized };
                    }
                }

                Log.Information($"[LOG INFORMATION] - Usuário recuperado com sucesso.\n");

                Log.Information($"[LOG INFORMATION] - Gerando token.\n");

                // Create token.
                var token = await _tokenService.CreateJsonWebToken(loginRequest.Username);

                Log.Information($"[LOG INFORMATION] - Token gerado {token.Value}.\n");

                // Response success.
                var apiResponseSuccess = new ApiResponse<object>(signInResult.Succeeded, token, new List<DadosNotificacao> { new DadosNotificacao(StatusCodes.SuccessOK, "Usuário autenticado com sucesso.") });

                // Return the token.
                return new ObjectResult(apiResponseSuccess) { StatusCode = (int)StatusCodes.SuccessOK };
            }
            catch (Exception exception)
            {
                Log.Error($"[LOG ERROR] - {exception.InnerException} - {exception.Message}\n");

                // Response error
                var apiResponseError = new ApiResponse<object>(false, new List<DadosNotificacao> { new DadosNotificacao(StatusCodes.ServerErrorInternalServerError, exception.Message) });

                // Return error.
                return new ObjectResult(apiResponseError) { StatusCode = (int)StatusCodes.ServerErrorInternalServerError };
            }
        }
        #endregion

        #region Create
        /// <summary>
        /// Método responsavel por criar um novo usuário.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ObjectResult> Create(PersonFastRequest personFastRequest)
        {
            Log.Information($"[LOG INFORMATION] - SET TITLE {nameof(UserService)} - METHOD {nameof(Create)}\n");

            try
            {
                Log.Information($"[LOG INFORMATION] - Validando request.\n");

                #region validate requests
                // Validate person reques.
                var validation = await new CreatePersonValidator().ValidateAsync(personFastRequest); if (validation.IsValid is false) return validation.CarregarErrosValidator();
                #endregion

                Log.Information($"[LOG INFORMATION] - Request validado com sucesso.\n");

                #region Conver request to identity
                // Convert request to user.
                var user = personFastRequest.User.ToIdentityUser();
                #endregion

                #region User create & set roles & claims
                // Build a user.
                var response = await BuildUser(user, personFastRequest.User);
                #endregion

                // Response succes true.
                if (response.Succeeded)
                {
                    #region Person create
                    // Create a new person.
                    var responsePerson = await _personService.Create(personFastRequest, user.Id);
                    #endregion

                    if (responsePerson.StatusCode == (int)StatusCodes.SuccessCreated)
                    {
                        #region Invite e-mail confirmation
                        // Confirm user for e-mail.
                        await ConfirmeUserForEmail(user);
                        #endregion

                        // Response success.
                        var apiResponseSuccess = new ApiResponse<object>(response.Succeeded, new List<DadosNotificacao> { new DadosNotificacao(StatusCodes.SuccessCreated, "Usuário criado com sucesso.") });

                        // Return response success.
                        return new ObjectResult(apiResponseSuccess) { StatusCode = (int)StatusCodes.SuccessCreated };
                    }
                    else
                    {
                        // Response not success.
                        var apiResponsePersonError = new ApiResponse<object>(response.Succeeded, response.Errors.Select(e => new DadosNotificacao(StatusCodes.ServerErrorInternalServerError, "Falha ao cadastra usuário. tente novamento")).ToList());

                        // Return response not success.
                        return new ObjectResult(apiResponsePersonError) { StatusCode = (int)StatusCodes.ServerErrorInternalServerError };
                    }
                }

                // Response not success.
                var apiResponseError = new ApiResponse<object>(response.Succeeded, response.Errors.Select(e => new DadosNotificacao(StatusCodes.ErrorBadRequest, e.Description)).ToList());

                // Return response not success.
                return new ObjectResult(apiResponseError) { StatusCode = (int)StatusCodes.ErrorBadRequest };
            }
            catch (Exception exception)
            {
                Log.Error($"[LOG ERROR] - {exception.InnerException} - {exception.Message}\n");

                // Create a apiResponse error.
                var apiResponseError = new ApiResponse<object>(false, new List<DadosNotificacao> { new DadosNotificacao(StatusCodes.ServerErrorInternalServerError, exception.Message) });

                // Return a apiResponse error.
                return new ObjectResult(apiResponseError) { StatusCode = (int)StatusCodes.ServerErrorInternalServerError };
            }
        }
        #endregion

        #region Activate
        /// <summary>
        /// Método responsavel por ativar um novo usuário.
        /// </summary>
        /// <param name="activateUserRequest"></param>
        /// <returns></returns>
        public async Task<ObjectResult> Activate(ActivateUserRequest activateUserRequest)
        {
            Log.Information($"[LOG INFORMATION] - SET TITLE {nameof(UserService)} - METHOD {nameof(Activate)}\n");

            try
            {
                Log.Information($"[LOG INFORMATION] - Recuperando usuário.\n");

                // Get user form Id.
                var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == activateUserRequest.UserId);

                Log.Information($"[LOG INFORMATION] - Confirmando e-mail do usuário {user.UserName}.\n");

                // Confirm e-mail user.
                var response = await _userManager.ConfirmEmailAsync(user, HttpUtility.UrlDecode(activateUserRequest.Code.Replace(";", "%")));

                // Response success true
                if (response.Succeeded)
                {
                    Log.Information($"[LOG INFORMATION] - Usuário ativado com sucesso.\n");

                    // Success response.
                    var apiResponseSuccess = new ApiResponse<object>(response.Succeeded, new List<DadosNotificacao> { new DadosNotificacao(DOMAIN.ENUM.StatusCodes.SuccessOK, "Usuário ativado com sucesso.") });

                    // Return response
                    return new ObjectResult(apiResponseSuccess) { StatusCode = (int)StatusCodes.SuccessOK };
                }

                Log.Information($"[LOG INFORMATION] - Falha na ativãção do usuário.\n");

                // Response not success.
                var apiResponseError = new ApiResponse<object>(response.Succeeded, new List<DadosNotificacao> { new DadosNotificacao(DOMAIN.ENUM.StatusCodes.ErrorBadRequest, "Falha ao ativar usuário.") });

                // Return response
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

        #region Roles & Claims
        /// <summary>
        /// Método responsavel por criar uma nova claim para o usuário.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ObjectResult> AddClaim(string username, ClaimRequest claimRequest)
        {
            Log.Information($"[LOG INFORMATION] - SET TITLE {nameof(UserService)} - METHOD {nameof(AddClaim)}\n");

            try
            {
                Log.Information($"[LOG INFORMATION] - Recuperando usuario {username}.\n");

                // Get user for Id.
                var user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName.Equals(username));

                Log.Information($"[LOG INFORMATION] - Adicionando a claim ({claimRequest.Type}/{claimRequest.Value}) no usuário.\n");

                #region User set claim
                // Add claim in user.
                var response = await _userManager.AddClaimAsync(user, new Claim(claimRequest.Type, claimRequest.Value));
                #endregion

                // Response success true.
                if (response.Succeeded)
                {
                    Log.Information($"[LOG INFORMATION] - Claim adicionada com sucesso.\n");

                    // Success response.
                    var apiResponseSuccess = new ApiResponse<object>(response.Succeeded, new List<DadosNotificacao> { new DadosNotificacao(StatusCodes.SuccessOK, $"Claim {claimRequest.Type} / {claimRequest.Value}, adicionada com sucesso ao usuário {username}.") });

                    // Return success response.
                    return new ObjectResult(apiResponseSuccess) { StatusCode = (int)StatusCodes.SuccessOK };
                }

                Log.Information($"[LOG INFORMATION] - Falha ao adicionar claim.\n");

                // Not success response.
                var apiResponseError = new ApiResponse<object>(response.Succeeded, response.Errors.Select(e => new DadosNotificacao(StatusCodes.ErrorBadRequest, e.Description)).ToList());

                // Return not success response.
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

        /// <summary>
        /// Método responsavel por remover uma claim do usuário.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="claimRequest"></param>
        /// <returns></returns>
        public async Task<ObjectResult> RemoveClaim(string username, ClaimRequest claimRequest)
        {
            Log.Information($"[LOG INFORMATION] - SET TITLE {nameof(UserService)} - METHOD {nameof(RemoveClaim)}\n");

            try
            {
                Log.Information($"[LOG INFORMATION] - Recuperando usuário {username}.\n");

                // Get user for Id.
                var user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName.Equals(username));

                Log.Information($"[LOG INFORMATION] - Removendo a claim ({claimRequest.Type}/{claimRequest.Value}) do usuário.\n");

                #region User remove claim
                // Remove claim.
                var response = await _userManager.RemoveClaimAsync(user, new Claim(claimRequest.Type, claimRequest.Value));
                #endregion

                // Response success true.
                if (response.Succeeded)
                {
                    Log.Information($"[LOG INFORMATION] - Claim remvida com sucesso.\n");

                    // Response success.
                    var apiResponseSuccess = new ApiResponse<object>(response.Succeeded, new List<DadosNotificacao> { new DadosNotificacao(DOMAIN.ENUM.StatusCodes.SuccessOK, $"Claim {claimRequest.Type} / {claimRequest.Value}, removida com sucesso do usuário {username}.") });

                    // Return response.
                    return new ObjectResult(apiResponseSuccess) { StatusCode = (int)StatusCodes.SuccessOK };
                }

                Log.Information($"[LOG INFORMATION] - Falha ao remover claim.\n");

                // Response not success
                var apiResponseError = new ApiResponse<object>(response.Succeeded, response.Errors.Select(e => new DadosNotificacao(DOMAIN.ENUM.StatusCodes.ErrorBadRequest, e.Description)).ToList());

                // Return response
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

        /// <summary>
        /// Método responsavel por adicionar uma role ao usuário.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public async Task<ObjectResult> AddRole(string username, string roleName)
        {
            Log.Information($"[LOG INFORMATION] - SET TITLE {nameof(UserService)} - METHOD {nameof(AddRole)}\n");

            try
            {
                Log.Information($"[LOG INFORMATION] - Recuperando dados do usuário {username}.\n");

                // Get user for Id.
                var user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName.Equals(username));

                Log.Information($"[LOG INFORMATION] - Adicionando a role ({roleName}) ao usuário.\n");

                #region User set claim
                // Add Role.
                var response = await _userManager.AddToRoleAsync(user, roleName);
                #endregion

                // Response success true.
                if (response.Succeeded)
                {
                    Log.Information($"[LOG INFORMATION] - Role adicionada com sucesso.\n");

                    // Response success.
                    var apiResponseSuccess = new ApiResponse<object>(response.Succeeded, new List<DadosNotificacao> { new DadosNotificacao(DOMAIN.ENUM.StatusCodes.SuccessOK, $"Role {roleName}, adicionada com sucesso ao usuário {username}.") });

                    // Return response.
                    return new ObjectResult(apiResponseSuccess) { StatusCode = (int)StatusCodes.SuccessOK };
                }

                Log.Information($"[LOG INFORMATION] - Falha ao adicionar role.\n");

                // Response not success
                var apiResponseError = new ApiResponse<object>(response.Succeeded, response.Errors.Select(e => new DadosNotificacao(DOMAIN.ENUM.StatusCodes.ErrorBadRequest, e.Description)).ToList());

                // Return response
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

        /// <summary>
        /// Método responsavel por remover uma role ao usuário.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="claimRequest"></param>
        /// <returns></returns>
        public async Task<ObjectResult> RemoveRole(string username, string roleName)
        {
            Log.Information($"[LOG INFORMATION] - SET TITLE {nameof(UserService)} - METHOD {nameof(RemoveRole)}\n");

            try
            {
                Log.Information($"[LOG INFORMATION] - Recuperando dados do usuário {username}.\n");

                // Get user for Id.
                var user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName.Equals(username));

                Log.Information($"[LOG INFORMATION] - Removendo role ({roleName}) do usuário.\n");

                #region User remove claim
                // Remove role.
                var response = await _userManager.RemoveFromRoleAsync(user, roleName);
                #endregion

                // Response success true.
                if (response.Succeeded)
                {
                    Log.Information($"[LOG INFORMATION] - Role removida com sucesso.\n");

                    // Response success
                    var apiResponseSuccess = new ApiResponse<object>(response.Succeeded, new List<DadosNotificacao> { new DadosNotificacao(DOMAIN.ENUM.StatusCodes.SuccessOK, $"Role {roleName}, removida com sucesso do usuário {username}.") });

                    // Return response.
                    return new ObjectResult(apiResponseSuccess) { StatusCode = (int)StatusCodes.SuccessOK };
                }

                Log.Information($"[LOG INFORMATION] - Falha ao remover role.\n");

                // Response not success.
                var apiResponseError = new ApiResponse<object>(response.Succeeded, response.Errors.Select(e => new DadosNotificacao(DOMAIN.ENUM.StatusCodes.ErrorBadRequest, e.Description)).ToList());

                // Return response.
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
                var identityResult = await _userManager.CreateAsync(user, userRequest.Password);

                if (identityResult.Succeeded)
                {
                    Log.Information($"[LOG INFORMATION] - Usuário criado com sucesso {user.UserName}, dados {JsonConvert.SerializeObject(userRequest)}\n");

                    // Add Login to user.
                    await _userManager.AddLoginAsync(user, new UserLoginInfo("TOOLS.USER.API", "TOOLS.USER", "TOOLS.USER.PROVIDER.KEY"));

                    return identityResult;
                }

                Log.Information($"[LOG INFORMATION] - Falha ao remover role.\n");

                // Return identity result.
                return identityResult;
            }
            catch (Exception exception)
            {
                Log.Error($"[LOG ERROR] - {exception.InnerException} - {exception.Message}\n");

                // Error.
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

                // Generate email code.
                var emailCode = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                // Codify email code.
                var codifyEmailCode = HttpUtility.UrlEncode(emailCode).Replace("%", ";");

                Log.Information($"[LOG INFORMATION] - Código gerado - {codifyEmailCode}.\n");

                // Invite email code.
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
                Log.Error($"[LOG ERROR] - {exception.InnerException} - {exception.Message}\n");

                // Error.
                throw new Exception(exception.Message);
            }
        }
        #endregion
    }
}
