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
using APPLICATION.DOMAIN.UTILS.EXTENSIONS;
using APPLICATION.DOMAIN.UTILS.GLOBAL;
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

                // sigin user wirh username & password.
                var signInResult = await _signInManager.PasswordSignInAsync(loginRequest.Username, loginRequest.Password, isPersistent: true, lockoutOnFailure: true);

                // return error response.
                if (signInResult.Succeeded is false)
                {
                    // locked user.
                    if (signInResult.IsLockedOut)
                    {
                        Log.Information($"[LOG INFORMATION] - Falha ao recuperar usuário, está bloqueado.\n");

                        // Response Locked.
                        return new ApiResponse<object>(signInResult.Succeeded, StatusCodes.ErrorLocked, new List<DadosNotificacao> { new DadosNotificacao("Usuário está bloqueado. Caso não desbloqueie em alguns minutos entre em contato com o suporte.") });
                    }
                    else if (signInResult.IsNotAllowed) // not allowed user.
                    {
                        Log.Information($"[LOG INFORMATION] - Falha ao recuperar usuário, não está confirmado.\n");

                        // Response notAllowed.
                        return new ApiResponse<object>(signInResult.Succeeded, StatusCodes.ErrorUnauthorized, new List<DadosNotificacao> { new DadosNotificacao("Email do usuário não está confirmado.") });
                    }
                    else if (signInResult.RequiresTwoFactor) // requires two factor user.
                    {
                        Log.Information($"[LOG INFORMATION] - Falha ao recuperar usuário, requer verificação de dois fatores.\n");

                        // Response twoFactor.
                        return new ApiResponse<object>(signInResult.Succeeded, StatusCodes.ErrorUnauthorized, new List<DadosNotificacao> { new DadosNotificacao("Usuário necessita de verificação de dois fatores.") });
                    }
                    else // incorrects params user.
                    {
                        Log.Information($"[LOG INFORMATION] - Falha ao recuperar usuário, dados incorretos.\n");

                        // Response error unathorized.
                        return new ApiResponse<object>(signInResult.Succeeded, StatusCodes.ErrorUnauthorized, new List<DadosNotificacao> { new DadosNotificacao("Os dados do usuário estão inválidos ou usuário não existe.") });
                    }
                }

                Log.Information($"[LOG INFORMATION] - Gerando token.\n");

                // Create token.
                var token = await _tokenService.CreateJsonWebToken(loginRequest.Username);

                // Response success.
                return new ApiResponse<object>(signInResult.Succeeded, StatusCodes.SuccessOK, token, new List<DadosNotificacao> { new DadosNotificacao("Usuário autenticado com sucesso.") });
            }
            catch (Exception exception)
            {
                Log.Error($"[LOG ERROR] - {exception.Message}\n");

                // Response error
                return new ApiResponse<object>(false, StatusCodes.ServerErrorInternalServerError, new List<DadosNotificacao> { new DadosNotificacao(exception.Message) });
            }
        }
        #endregion

        #region Create
        /// <summary>
        /// Método responsavel por criar um novo usuário.
        /// </summary>
        /// <param name="personFastRequest"></param>
        /// <returns></returns>
        public async Task<ApiResponse<object>> Create(PersonFastRequest personFastRequest)
        {
            Log.Information($"[LOG INFORMATION] - SET TITLE {nameof(UserService)} - METHOD {nameof(Create)}\n");

            try
            {
                Log.Information($"[LOG INFORMATION] - Validando request.\n");

                // Validate person reques.
                var validation = await new CreatePersonValidator().ValidateAsync(personFastRequest); if (validation.IsValid is false) return validation.CarregarErrosValidator();

                Log.Information($"[LOG INFORMATION] - Request validado com sucesso.\n");

                // Convert request to user.
                var user = personFastRequest.User.ToIdentityUser();

                // Build a user.
                var response = await BuildUser(user, personFastRequest.User);

                // Response succes true.
                if (response.Succeeded)
                {
                    // Create a new person.
                    var responsePerson = await _personService.Create(personFastRequest, user.Id);

                    // Is success...
                    if (responsePerson.Sucesso is true)
                    {
                        // Confirm user for e-mail.
                        await ConfirmeUserForEmail(user);

                        // Response success.
                        return new ApiResponse<object>(response.Succeeded, StatusCodes.SuccessCreated, responsePerson, new List<DadosNotificacao> { new DadosNotificacao("Usuário criado com sucesso.") });
                    }

                    // Response error.
                    return new ApiResponse<object>(response.Succeeded, StatusCodes.ServerErrorInternalServerError, new List<DadosNotificacao> { new DadosNotificacao("Ocorreu uma falha ao criar usuário.") });
                }

                // Response error.
                return new ApiResponse<object>(response.Succeeded, StatusCodes.ServerErrorInternalServerError, new List<DadosNotificacao> { new DadosNotificacao("Ocorreu uma falha ao criar usuário.") });
            }
            catch (Exception exception)
            {
                Log.Error($"[LOG ERROR] - {exception.Message}\n");

                // Response error.
                return new ApiResponse<object>(false, StatusCodes.ServerErrorInternalServerError, new List<DadosNotificacao> { new DadosNotificacao(exception.Message) });
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
                    return new ApiResponse<object>(response.Succeeded, StatusCodes.SuccessOK, new List<DadosNotificacao> { new DadosNotificacao("Usuário ativado com sucesso.") });
                }

                Log.Information($"[LOG INFORMATION] - Falha na ativãção do usuário.\n"); throw new Exception("Falha ao ativar usuário.");
            }
            catch (Exception exception)
            {
                Log.Error($"[LOG ERROR] - {exception.Message}\n");

                // Error response.
                return new ApiResponse<object>(false, StatusCodes.ServerErrorInternalServerError, new List<DadosNotificacao> { new DadosNotificacao(exception.Message) });
            }
        }
        #endregion

        #region Roles & Claims
        /// <summary>
        /// Método responsavel por criar uma nova claim para o usuário.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="claimRequest"></param>
        /// <returns></returns>
        public async Task<ApiResponse<object>> AddClaim(string username, ClaimRequest claimRequest)
        {
            Log.Information($"[LOG INFORMATION] - SET TITLE {nameof(UserService)} - METHOD {nameof(AddClaim)}\n");

            try
            {
                Log.Information($"[LOG INFORMATION] - Recuperando usuario {username}.\n");

                // Get user for Id.
                var user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName.Equals(username));

                Log.Information($"[LOG INFORMATION] - Adicionando a claim ({claimRequest.Type}/{claimRequest.Value}) no usuário.\n");

                // Add claim in user.
                var response = await _userManager.AddClaimAsync(user, new Claim(claimRequest.Type, claimRequest.Value));

                // Response success true.
                if (response.Succeeded)
                {
                    Log.Information($"[LOG INFORMATION] - Claim adicionada com sucesso.\n");

                    // Success response.
                    return new ApiResponse<object>(response.Succeeded, StatusCodes.SuccessOK, new List<DadosNotificacao> { new DadosNotificacao($"Claim {claimRequest.Type} / {claimRequest.Value}, adicionada com sucesso ao usuário {username}.") });
                }

                Log.Information($"[LOG ERROR] - Falha ao adicionar claim.\n"); throw new Exception("Falha ao adicionar claim.");
            }
            catch (Exception exception)
            {
                Log.Error($"[LOG ERROR] - {exception.Message}\n");

                // Error response.
                return new ApiResponse<object>(false, StatusCodes.ServerErrorInternalServerError, new List<DadosNotificacao> { new DadosNotificacao(exception.Message) });
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

                // Get user for Id.
                var user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName.Equals(username));

                Log.Information($"[LOG INFORMATION] - Removendo a claim ({claimRequest.Type}/{claimRequest.Value}) do usuário.\n");

                // Remove claim.
                var response = await _userManager.RemoveClaimAsync(user, new Claim(claimRequest.Type, claimRequest.Value));

                // Response success true.
                if (response.Succeeded)
                {
                    Log.Information($"[LOG INFORMATION] - Claim remvida com sucesso.\n");

                    // Response success.
                    return new ApiResponse<object>(response.Succeeded, StatusCodes.SuccessOK, new List<DadosNotificacao> { new DadosNotificacao($"Claim {claimRequest.Type} / {claimRequest.Value}, removida com sucesso do usuário {username}.") });
                }

                Log.Information($"[LOG ERROR] - Falha ao remover claim.\n"); throw new Exception("Falha ao remover claim.");
            }
            catch (Exception exception)
            {
                Log.Error($"[LOG ERROR] - {exception.Message}\n");

                // Error response.
                return new ApiResponse<object>(false, StatusCodes.ServerErrorInternalServerError, new List<DadosNotificacao> { new DadosNotificacao(exception.Message) });
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

                // Get user for Id.
                var user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName.Equals(username));

                Log.Information($"[LOG INFORMATION] - Adicionando a role ({roleName}) ao usuário.\n");

                // Add Role.
                var response = await _userManager.AddToRoleAsync(user, roleName);

                // Response success true.
                if (response.Succeeded)
                {
                    Log.Information($"[LOG INFORMATION] - Role adicionada com sucesso.\n");

                    // Response success.
                    return new ApiResponse<object>(response.Succeeded, StatusCodes.SuccessOK, new List<DadosNotificacao> { new DadosNotificacao($"Role {roleName}, adicionada com sucesso ao usuário {username}.") });
                }

                Log.Information($"[LOG ERROR] - Falha ao adicionar role.\n"); throw new Exception("Falha ao adicionar role.");
            }
            catch (Exception exception)
            {
                Log.Error($"[LOG ERROR] - {exception.Message}\n");

                // Error response.
                return new ApiResponse<object>(false, StatusCodes.ServerErrorInternalServerError, new List<DadosNotificacao> { new DadosNotificacao(exception.Message) });
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

                // Get user for Id.
                var user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName.Equals(username));

                Log.Information($"[LOG INFORMATION] - Removendo role ({roleName}) do usuário.\n");

                // Remove role.
                var response = await _userManager.RemoveFromRoleAsync(user, roleName);

                // Response success true.
                if (response.Succeeded)
                {
                    Log.Information($"[LOG INFORMATION] - Role removida com sucesso.\n");

                    // Response success
                    return new ApiResponse<object>(response.Succeeded, StatusCodes.SuccessOK, new List<DadosNotificacao> { new DadosNotificacao($"Role {roleName}, removida com sucesso do usuário {username}.") });
                }

                Log.Information($"[LOG INFORMATION] - Falha ao remover role.\n"); throw new Exception("Falha ao remover role.");
            }
            catch (Exception exception)
            {
                Log.Error($"[LOG ERROR] - {exception.Message}\n");

                // Error response.
                return new ApiResponse<object>(false, StatusCodes.ServerErrorInternalServerError, new List<DadosNotificacao> { new DadosNotificacao(exception.Message) });
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

                // Is not success...
                if (identityResult.Succeeded is false) throw new Exception("Falha ao criar usuário.");

                Log.Information($"[LOG INFORMATION] - Usuário criado com sucesso {user.UserName}, dados {JsonConvert.SerializeObject(userRequest)}\n");

                // Add Login to user.
                await _userManager.AddLoginAsync(user, new UserLoginInfo("TOOLS.USER.API", "TOOLS.USER", "TOOLS.USER.PROVIDER.KEY"));

                // Return result.
                return identityResult;

            }
            catch (Exception exception)
            {
                Log.Error($"[LOG ERROR] - {exception.Message}\n");

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
                // Error.
                throw new Exception(exception.Message, exception.InnerException);
            }
        }
        #endregion
    }
}
