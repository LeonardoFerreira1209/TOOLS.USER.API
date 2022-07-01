using APPLICATION.DOMAIN.CONTRACTS.SERVICES.USER;
using APPLICATION.DOMAIN.DTOS.CONFIGURATION;
using APPLICATION.DOMAIN.DTOS.CONFIGURATION.AUTH.TOKEN;
using APPLICATION.DOMAIN.DTOS.REQUEST;
using APPLICATION.DOMAIN.DTOS.REQUEST.USER;
using APPLICATION.DOMAIN.DTOS.RESPONSE;
using APPLICATION.INFRAESTRUTURE.FACADES.EMAIL;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Serilog;
using System.Web;

namespace APPLICATION.APPLICATION.SERVICES.USER
{
    /// <summary>
    /// Serviço de usuários.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly SignInManager<IdentityUser<Guid>> _signInManager;

        private readonly UserManager<IdentityUser<Guid>> _userManager;

        private readonly IOptions<AppSettings> _appsettings;

        private readonly EmailFacade _emailFacade;

        private readonly IMapper _mapper;
        public UserService(SignInManager<IdentityUser<Guid>> signInManager, UserManager<IdentityUser<Guid>> userManager, IOptions<AppSettings> appsettings, EmailFacade emailFacade, IMapper mapper)
        {
            _signInManager = signInManager;

            _userManager = userManager;

            _appsettings = appsettings;

            _emailFacade = emailFacade;

            _mapper = mapper;
        }

        /// <summary>
        /// Método responsável por fazer a authorização do usuário.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ApiResponse<TokenJWT>> Authentication(LoginRequest request)
        {
            Log.Information($"[LOG INFORMATION] - SET TITLE {nameof(UserService)} - METHOD {nameof(Authentication)}\n");

            try
            {
                var response = await _signInManager.PasswordSignInAsync(request.Username, request.Password, true, lockoutOnFailure: true);

                if (response.Succeeded)
                {
                    var token = new TokenJwtBuilder()
                    .AddSecurityKey(JwtSecurityKey.Create(_appsettings.Value.Auth.SecurityKey))
                    .AddSubject("HYPER.IO PROJECTS L.T.D.A")
                    .AddIssuer(_appsettings.Value.Auth.ValidIssuer)
                    .AddAudience(_appsettings.Value.Auth.ValidAudience)
                    .AddClaim("Admin", "1")
                    .AddExpiry(_appsettings.Value.Auth.ExpiresIn)
                    .Builder();

                    return new ApiResponse<TokenJWT>(response.Succeeded, token);
                }

                return new ApiResponse<TokenJWT>(response.Succeeded, new List<DadosNotificacao> { new DadosNotificacao(DOMAIN.ENUM.StatusCodes.ErrorUnauthorized, "Usuário não autorizado.") });
            }
            catch (Exception exception)
            {
                Log.Error("[LOG ERROR]", exception, exception.Message);

                return new ApiResponse<TokenJWT>(false, new List<DadosNotificacao> { new DadosNotificacao(DOMAIN.ENUM.StatusCodes.ServerErrorInternalServerError, exception.Message) });
            }
        }

        /// <summary>
        /// Método responsavel por criar um novo usuário.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ApiResponse<TokenJWT>> Create(CreateRequest request)
        {
            Log.Information($"[LOG INFORMATION] - SET TITLE {nameof(UserService)} - METHOD {nameof(Create)}\n");

            try
            {
                var identityUser = _mapper.Map<IdentityUser<Guid>>(request);

                var response = await _userManager.CreateAsync(identityUser, request.Password);

                if (response.Succeeded)
                {
                    await ConfirmeUserForEmail(identityUser);

                    return new ApiResponse<TokenJWT>(response.Succeeded, new List<DadosNotificacao> { new DadosNotificacao(DOMAIN.ENUM.StatusCodes.SuccessCreated, "Usuário criado com sucesso.") });
                }

                return new ApiResponse<TokenJWT>(response.Succeeded, response.Errors.Select(e => new DadosNotificacao(DOMAIN.ENUM.StatusCodes.ErrorBadRequest, e.Description)).ToList());
            }
            catch (Exception exception)
            {
                Log.Error("[LOG ERROR]", exception, exception.Message);

                return new ApiResponse<TokenJWT>(false, new List<DadosNotificacao> { new DadosNotificacao(DOMAIN.ENUM.StatusCodes.ServerErrorInternalServerError, exception.Message) });
            }
        }

        /// <summary>
        /// Método responsavel por gerar um token de autorização e enviar por e-mail.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private async Task ConfirmeUserForEmail(IdentityUser<Guid> user)
        {
            var emailCode = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            var codifyEmailCode = HttpUtility.UrlEncode(emailCode).Replace("%", ";");

            _emailFacade.Invite(new MailRequest
            {

                Receivers = new List<string> { user.Email },
                Link = $"{_appsettings.Value.UrlBase.BASE_URL}/security/activate/{codifyEmailCode}/{user.Id}",
                Subject = "Ativação de e-mail",
                Content = $"Olá {user.UserName}, estamos muito felizes com o seu cadastro em nosso sistema. Clique no botão para liberarmos o seu acesso.",
                ButtonText = "Clique para ativar o e-mail",
                TemplateName = "Welcome.Template"

            });
        }

        /// <summary>
        /// Método responsavel por ativar um novo usuário.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ApiResponse<TokenJWT>> Activate(ActivateUserRequest request)
        {
            Log.Information($"[LOG INFORMATION] - SET TITLE {nameof(UserService)} - METHOD {nameof(Activate)}\n");

            try
            {
                var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == request.UsuarioId);

                var response = await _userManager.ConfirmEmailAsync(user, HttpUtility.UrlDecode(request.Codigo.Replace(";", "%")));

                if (response.Succeeded) return new ApiResponse<TokenJWT>(response.Succeeded, new List<DadosNotificacao> { new DadosNotificacao(DOMAIN.ENUM.StatusCodes.SuccessOK, "Usuário ativado com sucesso.") });

                return new ApiResponse<TokenJWT>(response.Succeeded, new List<DadosNotificacao> { new DadosNotificacao(DOMAIN.ENUM.StatusCodes.ErrorBadRequest, "Falha ao ativar usuário.") });
            }
            catch (Exception exception)
            {
                Log.Error("[LOG ERROR]", exception, exception.Message);

                return new ApiResponse<TokenJWT>(false, new List<DadosNotificacao> { new DadosNotificacao(DOMAIN.ENUM.StatusCodes.ServerErrorInternalServerError, exception.Message) });
            }
        }

    }
}
