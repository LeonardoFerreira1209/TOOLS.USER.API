using APPLICATION.APPLICATION.CONFIGURATIONS;
using APPLICATION.DOMAIN.CONTRACTS.SERVICES.TOKEN;
using APPLICATION.DOMAIN.CONTRACTS.SERVICES.USER;
using APPLICATION.DOMAIN.DTOS.CONFIGURATION;
using APPLICATION.DOMAIN.DTOS.REQUEST;
using APPLICATION.DOMAIN.DTOS.REQUEST.USER;
using APPLICATION.DOMAIN.DTOS.RESPONSE.USER.ROLE;
using APPLICATION.DOMAIN.DTOS.RESPONSE.UTILS;
using APPLICATION.DOMAIN.ENTITY.ROLE;
using APPLICATION.DOMAIN.ENTITY.USER;
using APPLICATION.DOMAIN.ENUM;
using APPLICATION.DOMAIN.UTILS.Extensions;
using APPLICATION.DOMAIN.UTILS.EXTENSIONS;
using APPLICATION.DOMAIN.UTILS.GLOBAL;
using APPLICATION.DOMAIN.VALIDATORS;
using APPLICATION.INFRAESTRUTURE.FACADES.EMAIL;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Serilog;
using System.Data;
using System.Security.Claims;
using System.Web;

namespace APPLICATION.APPLICATION.SERVICES.USER
{
    /// <summary>
    /// Serviço de usuários.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly SignInManager<UserEntity> _signInManager;

        private readonly UserManager<UserEntity> _userManager;

        private readonly RoleManager<RoleEntity> _roleManager;

        private readonly IOptions<AppSettings> _appsettings;

        private readonly EmailFacade _emailFacade;

        private readonly ITokenService _tokenService;

        public UserService(SignInManager<UserEntity> signInManager, UserManager<UserEntity> userManager, RoleManager<RoleEntity> roleManager, IOptions<AppSettings> appsettings, EmailFacade emailFacade, ITokenService tokenService)
        {
            _signInManager = signInManager;

            _userManager = userManager;

            _roleManager = roleManager;

            _appsettings = appsettings;

            _emailFacade = emailFacade;

            _tokenService = tokenService;
        }

        /// <summary>
        /// Método responsável por fazer a authorização do usuário.
        /// </summary>
        /// <param name="loginRequest"></param>
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
                        return new ApiResponse<object>(signInResult.Succeeded, StatusCodes.ErrorLocked, null, new List<DadosNotificacao> { new DadosNotificacao("Usuário está bloqueado. Caso não desbloqueie em alguns minutos entre em contato com o suporte.") });
                    }
                    else if (signInResult.IsNotAllowed) // not allowed user.
                    {
                        Log.Information($"[LOG INFORMATION] - Falha ao recuperar usuário, não está confirmado.\n");

                        // Response notAllowed.
                        return new ApiResponse<object>(signInResult.Succeeded, StatusCodes.ErrorUnauthorized, null, new List<DadosNotificacao> { new DadosNotificacao("Email do usuário não está confirmado.") });
                    }
                    else if (signInResult.RequiresTwoFactor) // requires two factor user.
                    {
                        Log.Information($"[LOG INFORMATION] - Falha ao recuperar usuário, requer verificação de dois fatores.\n");

                        // Response twoFactor.
                        return new ApiResponse<object>(signInResult.Succeeded, StatusCodes.ErrorUnauthorized, null, new List<DadosNotificacao> { new DadosNotificacao("Usuário necessita de verificação de dois fatores.") });
                    }
                    else // incorrects params user.
                    {
                        Log.Information($"[LOG INFORMATION] - Falha ao recuperar usuário, dados incorretos.\n");

                        // Response error unathorized.
                        return new ApiResponse<object>(signInResult.Succeeded, StatusCodes.ErrorUnauthorized, null, new List<DadosNotificacao> { new DadosNotificacao("Os dados do usuário estão inválidos ou usuário não existe.") });
                    }
                }

                Log.Information($"[LOG INFORMATION] - Gerando token.\n");

                return await _tokenService.CreateJsonWebToken(loginRequest.Username);
            }
            catch (Exception exception)
            {
                Log.Error($"[LOG ERROR] - {exception.Message}\n");

                // Response error
                return new ApiResponse<object>(false, StatusCodes.ServerErrorInternalServerError, null, new List<DadosNotificacao> { new DadosNotificacao(exception.Message) });
            }
        }

        /// <summary>
        /// Método responsavel por criar um novo usuário.
        /// </summary>
        /// <param name="userCreateRequest"></param>
        /// <returns></returns>
        public async Task<ApiResponse<object>> Create(UserCreateRequest userCreateRequest)
        {
            Log.Information($"[LOG INFORMATION] - SET TITLE {nameof(UserService)} - METHOD {nameof(Create)}\n");

            try
            {
                Log.Information($"[LOG INFORMATION] - Validando request.\n");

                // Validate person reques.
                var validation = await new CreateUserValidator().ValidateAsync(userCreateRequest); if (validation.IsValid is false) return validation.CarregarErrosValidator();

                Log.Information($"[LOG INFORMATION] - Request validado com sucesso.\n");

                var user = userCreateRequest.ToIdentityUser();

                // Build a user.
                var response = await BuildUser(user, userCreateRequest.Password);

                // Response succes true.
                if (response.Succeeded)
                {
                    // Confirm user for e-mail.
                    await ConfirmeUserForEmail(user);

                    // Response success.
                    return new ApiResponse<object>(response.Succeeded, StatusCodes.SuccessCreated, null, new List<DadosNotificacao> { new DadosNotificacao("Usuário criado com sucesso.") });
                }

                // Response error.
                return new ApiResponse<object>(response.Succeeded, StatusCodes.ErrorBadRequest, null, response.Errors.Select((e) => new DadosNotificacao(e.Code.CustomExceptionMessage())).ToList());
            }
            catch (Exception exception)
            {
                Log.Error($"[LOG ERROR] - {exception.Message}\n");

                // Response error.
                return new ApiResponse<object>(false, StatusCodes.ServerErrorInternalServerError, null, new List<DadosNotificacao> { new DadosNotificacao(exception.Message) });
            }
        }

        /// <summary>
        /// Método responsável por atualizar um usuário.
        /// </summary>
        /// <param name="userUpdateRequest"></param>
        /// <returns></returns>
        public async Task<ApiResponse<object>> Update(UserUpdateRequest userUpdateRequest)
        {
            Log.Information($"[LOG INFORMATION] - SET TITLE {nameof(UserService)} - METHOD {nameof(Update)}\n");

            try
            {
                Log.Information($"[LOG INFORMATION] - Validando request.\n");

                // Validate de userRequest.
                var validation = await new UpdateUserValidator().ValidateAsync(userUpdateRequest); if (validation.IsValid is false) return validation.CarregarErrosValidator();

                Log.Information($"[LOG INFORMATION] - Request validado com sucesso.\n");

                Log.Information($"[LOG INFORMATION] - Atualizando dados do usuário {JsonConvert.SerializeObject(userUpdateRequest)}.\n");

                // Get user.
                var user = await _userManager.Users.FirstOrDefaultAsync(user => user.UserName.Equals(userUpdateRequest.UserName));

                // User is valid ?.
                if(user is not null)
                {
                    // Complete user.
                    user = userUpdateRequest.ToCompleteUserUpdateWithRequest(user);

                    // update username.
                    var setUsernameResponse = await _userManager.SetUserNameAsync(user, userUpdateRequest.UserName);

                    if (setUsernameResponse.Succeeded is false)
                    {
                        Log.Information($"[LOG INFORMATION] - Erro ao atualizar nome de usuário.\n");

                        return new ApiResponse<object>(false, StatusCodes.ErrorBadRequest, null, new List<DadosNotificacao> { new DadosNotificacao("Falha ao atualizar nome de usuário.") });
                    }

                    // update password.
                    if (!string.IsNullOrEmpty(userUpdateRequest.Password))
                    {
                        var changePasswordResponse = await _userManager.ChangePasswordAsync(user, userUpdateRequest.CurrentPassword, userUpdateRequest.Password);

                        if (changePasswordResponse.Succeeded is false)
                        {
                            Log.Information($"[LOG INFORMATION] - Erro ao trocar senha.\n");

                            return new ApiResponse<object>(false, StatusCodes.ErrorBadRequest, null, new List<DadosNotificacao> { new DadosNotificacao(changePasswordResponse.Errors.FirstOrDefault().Code.CustomExceptionMessage()) });
                        }
                    }

                    // update e-mail.
                    var setEmailResponse = await _userManager.SetEmailAsync(user, userUpdateRequest.Email);

                    if (setEmailResponse.Succeeded is false)
                    {
                        Log.Information($"[LOG INFORMATION] - Erro ao atualizar e-mail de usuário.\n");

                        return new ApiResponse<object>(false, StatusCodes.ErrorBadRequest, null, new List<DadosNotificacao> { new DadosNotificacao("Falha ao atualizar e-mail do usuário.") });
                    }

                    // update phoneNumber
                    var setPhoneNumberResponse = await _userManager.SetPhoneNumberAsync(user, userUpdateRequest.PhoneNumber);

                    if (setPhoneNumberResponse.Succeeded is false)
                    {
                        Log.Information($"[LOG INFORMATION] - Erro ao atualizar celular do usuário.\n");

                        return new ApiResponse<object>(false, StatusCodes.ErrorBadRequest, null, new List<DadosNotificacao> { new DadosNotificacao("Falha ao atualizar celular do usuário.") });
                    }

                    // update user.
                    await _userManager.UpdateAsync(user);

                    Log.Information($"[LOG INFORMATION] - Usuário atualizado com sucesso.\n");

                    // Response success.
                    return new ApiResponse<object>(true, StatusCodes.SuccessOK, null, new List<DadosNotificacao> { new DadosNotificacao("Usuário atualizado com sucesso.") });
                }
                else
                {
                    Log.Information($"[LOG INFORMATION] - Usuário não encontrado.\n");

                    // Response success.
                    return new ApiResponse<object>(false, StatusCodes.ErrorBadRequest, userUpdateRequest, new List<DadosNotificacao> { new DadosNotificacao("Usuário não encontrado..") });
                }
            }
            catch (Exception exception)
            {
                Log.Error($"[LOG ERROR] - {exception.Message}\n");

                // Response error
                return new ApiResponse<object>(false, StatusCodes.ServerErrorInternalServerError, null, new List<DadosNotificacao> { new DadosNotificacao(exception.Message) });
            }
        }

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
                    return new ApiResponse<object>(response.Succeeded, StatusCodes.SuccessOK, null, new List<DadosNotificacao> { new DadosNotificacao("Usuário ativado com sucesso.") });
                }

                Log.Information($"[LOG INFORMATION] - Falha na ativãção do usuário.\n");

                return new ApiResponse<object>(false, StatusCodes.ServerErrorInternalServerError, null, new List<DadosNotificacao> { new DadosNotificacao("Falha na ativãção do usuário!") });
            }
            catch (Exception exception)
            {
                Log.Error($"[LOG ERROR] - {exception.Message}\n");

                // Error response.
                return new ApiResponse<object>(false, StatusCodes.ServerErrorInternalServerError, null, new List<DadosNotificacao> { new DadosNotificacao(exception.Message) });
            }
        }

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
                    return new ApiResponse<object>(response.Succeeded, StatusCodes.SuccessOK, null, new List<DadosNotificacao> { new DadosNotificacao($"Claim {claimRequest.Type} / {claimRequest.Value}, adicionada com sucesso ao usuário {username}.") });
                }

                Log.Information($"[LOG ERROR] - Falha ao adicionar claim.\n");

                // Response error.
                return new ApiResponse<object>(response.Succeeded, StatusCodes.ServerErrorInternalServerError, null, new List<DadosNotificacao> { new DadosNotificacao("Falha ao adicionar claim!") });
            }
            catch (Exception exception)
            {
                Log.Error($"[LOG ERROR] - {exception.Message}\n");

                // Error response.
                return new ApiResponse<object>(false, StatusCodes.ServerErrorInternalServerError, null, new List<DadosNotificacao> { new DadosNotificacao(exception.Message) });
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
                    return new ApiResponse<object>(response.Succeeded, StatusCodes.SuccessOK, null, new List<DadosNotificacao> { new DadosNotificacao($"Claim {claimRequest.Type} / {claimRequest.Value}, removida com sucesso do usuário {username}.") });
                }

                Log.Information($"[LOG ERROR] - Falha ao remover claim.\n");

                // Response error.
                return new ApiResponse<object>(response.Succeeded, StatusCodes.ServerErrorInternalServerError, null, new List<DadosNotificacao> { new DadosNotificacao("Falha ao remover claim!") });
            }
            catch (Exception exception)
            {
                Log.Error($"[LOG ERROR] - {exception.Message}\n");

                // Error response.
                return new ApiResponse<object>(false, StatusCodes.ServerErrorInternalServerError, null, new List<DadosNotificacao> { new DadosNotificacao(exception.Message) });
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
                    return new ApiResponse<object>(response.Succeeded, StatusCodes.SuccessOK, null, new List<DadosNotificacao> { new DadosNotificacao($"Role {roleName}, adicionada com sucesso ao usuário {username}.") });
                }

                Log.Information($"[LOG ERROR] - Falha ao adicionar role.\n");

                // Response error.
                return new ApiResponse<object>(response.Succeeded, StatusCodes.ServerErrorInternalServerError, null, new List<DadosNotificacao> { new DadosNotificacao("Falha ao adicionar role!") });
            }
            catch (Exception exception)
            {
                Log.Error($"[LOG ERROR] - {exception.Message}\n");

                // Error response.
                return new ApiResponse<object>(false, StatusCodes.ServerErrorInternalServerError, new List<DadosNotificacao> { new DadosNotificacao(exception.Message) });
            }
        }

        /// <summary>
        /// Método responsável por recuperar roles de um usuário.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<ApiResponse<object>> GetUserRoles(Guid userId)
        {
            Log.Information($"[LOG INFORMATION] - SET TITLE {nameof(RoleService)} - METHOD {nameof(GetUserRoles)}\n");

            try
            {
                Log.Information($"[LOG INFORMATION] - Recuperando todas as roles do usuário.\n");

                // Get user for Id.
                var user = _userManager.Users.FirstAsync(user => user.Id.Equals(userId)).Result;

                // Get roles.
                var response = await _userManager.GetRolesAsync(user);

                // Instance of role response
                var roles = new List<RolesResponse>();

                // for in response
                foreach (var roleName in response)
                {
                    // get role.
                    var roleEntity = await _roleManager.Roles.FirstOrDefaultAsync(role => role.Name.Equals(roleName));

                    // get role claims
                    var roleClaims = await _roleManager.GetClaimsAsync(roleEntity);

                    // add roles.
                    roles.Add(new RolesResponse
                    {

                        Name = roleName,
                        Claims = roleClaims
                    });
                }

                Log.Information($"[LOG INFORMATION] - Roles recuperadas.\n");

                // Response success.
                return new ApiResponse<object>(true, StatusCodes.SuccessOK, roles.ToList(), new List<DadosNotificacao> { new DadosNotificacao("Roles recuperadas com sucesso.") });
            }
            catch (Exception exception)
            {
                Log.Error($"[LOG ERROR] - {exception.InnerException} - {exception.Message}\n");

                // Error response.
                return new ApiResponse<object>(false, StatusCodes.ServerErrorInternalServerError, null, new List<DadosNotificacao> { new DadosNotificacao(exception.Message) });
            }
        }

        /// <summary>
        /// Método responsavel por remover uma role ao usuário.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="roleName"></param>
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
                    return new ApiResponse<object>(response.Succeeded, StatusCodes.SuccessOK, null, new List<DadosNotificacao> { new DadosNotificacao($"Role {roleName}, removida com sucesso do usuário {username}.") });
                }

                Log.Information($"[LOG INFORMATION] - Falha ao remover role.\n");

                // Response error.
                return new ApiResponse<object>(response.Succeeded, StatusCodes.ServerErrorInternalServerError, null, new List<DadosNotificacao> { new DadosNotificacao("Falha ao remover role.!") });
            }
            catch (Exception exception)
            {
                Log.Error($"[LOG ERROR] - {exception.Message}\n");

                // Error response.
                return new ApiResponse<object>(false, StatusCodes.ServerErrorInternalServerError, null, new List<DadosNotificacao> { new DadosNotificacao(exception.Message) });
            }
        }

        /// <summary>
        /// Método responsavel por gerar um usuário e vincular roles e claims.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private async Task<IdentityResult> BuildUser(UserEntity user, string password)
        {
            Log.Information($"[LOG INFORMATION] - SET TITLE {nameof(UserService)} - METHOD {nameof(BuildUser)}\n");

            Log.Information("[LOG INFORMATION] - Criando usuário\n");

            // Create User.
            var identityResult = await _userManager.CreateAsync(user, password);

            // Logged user.
            var responsibleUser = GlobalData<object>.GlobalUser?.Id;

            // responsible user is not null use he, is null use user created Id. 
            user.CreatedUserId = responsibleUser is not null ? responsibleUser.Value : user.Id;

            // update user with created Id seted.
            await _userManager.UpdateAsync(user);

            // Return result.
            return identityResult;
        }

        /// <summary>
        /// Método responsavel por gerar um token de autorização e enviar por e-mail.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private async Task ConfirmeUserForEmail(UserEntity user)
        {
            Log.Information($"[LOG INFORMATION] - SET TITLE {nameof(UserService)} - METHOD {nameof(ConfirmeUserForEmail)}\n");

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
                Link = $"{_appsettings.Value.UrlBase.TOOLS_WEB_APP}/confirmEmail/{codifyEmailCode}/{user.Id}",
                Subject = "Ativação de e-mail",
                Content = $"{user.UserName}, estamos muito felizes com o seu cadastro em nosso sistema. Clique no botão para liberarmos o seu acesso.",
                ButtonText = "Liberar acesso",
                TemplateName = "Activate.Template"
            });
        }
    }
}
