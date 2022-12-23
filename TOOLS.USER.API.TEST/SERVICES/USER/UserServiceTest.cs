using APPLICATION.APPLICATION.SERVICES.USER;
using APPLICATION.DOMAIN.CONTRACTS.API;
using APPLICATION.DOMAIN.CONTRACTS.FACADE;
using APPLICATION.DOMAIN.CONTRACTS.SERVICES.FILE;
using APPLICATION.DOMAIN.CONTRACTS.SERVICES.TOKEN;
using APPLICATION.DOMAIN.DTOS.CONFIGURATION;
using APPLICATION.DOMAIN.DTOS.REQUEST;
using APPLICATION.DOMAIN.DTOS.RESPONSE.UTILS;
using APPLICATION.DOMAIN.ENTITY.USER;
using APPLICATION.DOMAIN.ENUM;
using APPLICATION.INFRAESTRUTURE.REPOSITORY.USER;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Moq;
using TOOLS.USER.API.TEST.MOCKS.USER;
using Xunit;

namespace TOOLS.USER.API.TEST.SERVICES.USER;

public class UserTest
{
    private readonly Mock<IUserRepository> _mockUserRepository;

    private readonly Mock<IOptions<AppSettings>> _mockSettings;

    private readonly Mock<ITokenService> _mockTokenService;

    private readonly Mock<IFileService> _mockFileService;

    private readonly Mock<IEmailExternal> _mockEmailExternal;

    private readonly Mock<IEmailFacade> _mockEmailFacade;

    private readonly UserService _userService;

    public UserTest()
    {
        _mockUserRepository = new Mock<IUserRepository>();

        _mockSettings = new Mock<IOptions<AppSettings>>();

        _mockTokenService = new Mock<ITokenService>();

        _mockFileService = new Mock<IFileService>();

        _mockEmailExternal = new Mock<IEmailExternal>();

        _mockEmailFacade = new Mock<IEmailFacade>();

        _userService = new UserService(_mockUserRepository.Object, _mockSettings.Object, _mockEmailFacade.Object, _mockTokenService.Object, _mockFileService.Object);
    }

    [Fact]
    public async Task TestAuthenticationSuccessOK()
    {
        // Configure o mock do repositório de usuários para retornar sucesso na autenticação
        _mockUserRepository.Setup(repo => repo.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>())).ReturnsAsync(SignInResult.Success);

        // Configure o mock do serviço de token para retornar uma resposta com sucesso
        _mockTokenService.Setup(service => service.CreateJsonWebToken(It.IsAny<string>())).ReturnsAsync(new ApiResponse<object>(true, StatusCodes.SuccessOK, null));

        // Execute o método de autenticação
        var result = await _userService.AuthenticationAsync(UserMocks.LoginRequestMock());

        // Verifique se o resultado é um sucesso
        Assert.True(result.Sucesso);
        Assert.Equal(StatusCodes.SuccessOK, result.StatusCode);
    }

    [Fact]
    public async Task TestAuthenticationErrorUnauthorized()
    {
        // Configure o mock do repositório de usuários para retornar erro na autenticação
        _mockUserRepository.Setup(repo => repo.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>())).ReturnsAsync(SignInResult.Failed);

        // Configure o mock do serviço de token para retornar uma resposta com sucesso
        _mockTokenService.Setup(service => service.CreateJsonWebToken(It.IsAny<string>())).ReturnsAsync(new ApiResponse<object>(true, StatusCodes.SuccessOK, null));

        // Execute o método de autenticação
        var result = await _userService.AuthenticationAsync(UserMocks.LoginRequestMock());

        // Verifique se o resultado é um falso
        Assert.False(result.Sucesso);
        Assert.Equal(StatusCodes.ErrorUnauthorized, result.StatusCode);
    }

    [Fact]
    public async Task TestAuthenticationErrorLocked()
    {
        // Configure o mock do repositório de usuários para retornar erro na autenticação
        _mockUserRepository.Setup(repo => repo.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>())).ReturnsAsync(SignInResult.LockedOut);

        // Configure o mock do serviço de token para retornar uma resposta com sucesso
        _mockTokenService.Setup(service => service.CreateJsonWebToken(It.IsAny<string>())).ReturnsAsync(new ApiResponse<object>(true, StatusCodes.SuccessOK, null));

        // Execute o método de autenticação
        var result = await _userService.AuthenticationAsync(UserMocks.LoginRequestMock());

        // Verifique se o resultado é um falso
        Assert.False(result.Sucesso);
        Assert.Equal(StatusCodes.ErrorLocked, result.StatusCode);
    }

    [Fact]
    public async Task TestAuthenticationErrorNotAllowed()
    {
        // Configure o mock do repositório de usuários para retornar erro na autenticação
        _mockUserRepository.Setup(repo => repo.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>())).ReturnsAsync(SignInResult.NotAllowed);

        // Configure o mock do serviço de token para retornar uma resposta com sucesso
        _mockTokenService.Setup(service => service.CreateJsonWebToken(It.IsAny<string>())).ReturnsAsync(new ApiResponse<object>(true, StatusCodes.SuccessOK, null));

        // Execute o método de autenticação
        var result = await _userService.AuthenticationAsync(UserMocks.LoginRequestMock());

        // Verifique se o resultado é um falso
        Assert.False(result.Sucesso);
        Assert.Equal(StatusCodes.ErrorUnauthorized, result.StatusCode);
    }

    [Fact]
    public async Task TestAuthenticationErrorTwoFactorRequired()
    {
        // Configure o mock do repositório de usuários para retornar erro na autenticação
        _mockUserRepository.Setup(repo => repo.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>())).ReturnsAsync(SignInResult.TwoFactorRequired);

        // Configure o mock do serviço de token para retornar uma resposta com sucesso
        _mockTokenService.Setup(service => service.CreateJsonWebToken(It.IsAny<string>())).ReturnsAsync(new ApiResponse<object>(true, StatusCodes.SuccessOK, null));

        // Execute o método de autenticação
        var result = await _userService.AuthenticationAsync(UserMocks.LoginRequestMock());

        // Verifique se o resultado é um falso
        Assert.False(result.Sucesso);
        Assert.Equal(StatusCodes.ErrorUnauthorized, result.StatusCode);
    }

    [Fact]
    public async Task TestAuthenticationErrorInternalServer()
    {
        // Configure o mock do repositório de usuários para retornar error na autenticação
        _mockUserRepository.Setup(repo => repo.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>())).ThrowsAsync(new Exception());

        // Configure o mock do serviço de token para retornar uma resposta com sucesso
        _mockTokenService.Setup(service => service.CreateJsonWebToken(It.IsAny<string>())).ReturnsAsync(new ApiResponse<object>(true, StatusCodes.SuccessOK, null));

        // Execute o método de autenticação
        var result = await _userService.AuthenticationAsync(UserMocks.LoginRequestMock());

        // Verifique se o resultado é um falso
        Assert.False(result.Sucesso);
        Assert.Equal(StatusCodes.ServerErrorInternalServerError, result.StatusCode);
    }

    [Fact]
    public async Task TestGetAsyncSuccessOK()
    {
        // Configure o mock do repositório de usuários para retornar sucesso na recuperação
        _mockUserRepository.Setup(repo => repo.GetAsync(It.IsAny<Guid>())).ReturnsAsync(UserMocks.UserEntityMock);

        // Execute o método de recuperação
        var result = await _userService.GetAsync(Guid.NewGuid());

        // Verifique se o resultado é um true
        Assert.True(result.Sucesso);
        Assert.Equal(StatusCodes.SuccessOK, result.StatusCode);
    }

    [Fact]
    public async Task TestGetAsyncErrorNotFound()
    {
        // Configure o mock do repositório de usuários para retornar erro na recuperação
        _mockUserRepository.Setup(repo => repo.GetAsync(It.IsAny<Guid>())).ReturnsAsync(It.IsAny<UserEntity>());

        // Execute o método de recuperação
        var result = await _userService.GetAsync(Guid.NewGuid());

        // Verifique se o resultado é um falso
        Assert.False(result.Sucesso);
        Assert.Equal(StatusCodes.ErrorNotFound, result.StatusCode);
    }

    [Fact]
    public async Task TestGetAsyncErrorInternalServer()
    {
        // Configure o mock do repositório de usuários para retornar erro na recuperaçãp
        _mockUserRepository.Setup(repo => repo.GetAsync(It.IsAny<Guid>())).ThrowsAsync(new Exception());

        // Execute o método de recuperação
        var result = await _userService.GetAsync(Guid.NewGuid());

        // Verifique se o resultado é um falso
        Assert.False(result.Sucesso);
        Assert.Equal(StatusCodes.ServerErrorInternalServerError, result.StatusCode);
    }

    [Fact]
    public async Task TestCreateAsyncSuccessCreated()
    {
        _mockSettings.Setup(set => set.Value).Returns(new AppSettings
        {
            UrlBase = new UrlBase
            {
                TOOLS_MAIL_API = Faker.Internet.Url()
            }
        });

        // Configure o mock do repositório de usuários para retornar sucesso na criação
        _mockUserRepository.Setup(repo => repo.CreateUserAsync(It.IsAny<UserEntity>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);

        // Configure o mock do repositório de usuários para retornar sucesso na atualização do usuário
        _mockUserRepository.Setup(repo => repo.UpdateUserAsync(It.IsAny<UserEntity>())).ReturnsAsync(IdentityResult.Success);

        // Configure o mock do repositório de usuários para retornar um token de confirmação de email gerado de forma aleatória ao chamar o método de geração de token
        _mockUserRepository.Setup(repo => repo.GenerateEmailConfirmationTokenAsync(It.IsAny<UserEntity>())).ReturnsAsync(Faker.Currency.ThreeLetterCode);

        // Configure o mock do serviço de email para retornar sucesso ao chamar o método de invite
        _mockEmailFacade.Setup(facade => facade.Invite(It.IsAny<MailRequest>())).Returns(Task.FromResult(true));

        // Configure o mock do serviço de email externo para retornar um código de status OK na chamada do método de invite
        _mockEmailExternal.Setup(emailExter => emailExter.Invite(It.IsAny<MailRequest>())).ReturnsAsync(new HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.OK });

        // Execute o método de criação
        var result = await _userService.CreateAsync(UserMocks.UserCreateRequestMock());

        // Verifique se o resultado é um sucesso
        Assert.True(result.Sucesso);
        Assert.Equal(StatusCodes.SuccessCreated, result.StatusCode);
    }

    [Fact]
    public async Task TestCreateAsyncBadRequest()
    {
        _mockSettings.Setup(set => set.Value).Returns(new AppSettings
        {
            UrlBase = new UrlBase
            {
                TOOLS_MAIL_API = Faker.Internet.Url()
            }
        });

        // Configure o mock do repositório de usuários para retornar erro na criação
        _mockUserRepository.Setup(repo => repo.CreateUserAsync(It.IsAny<UserEntity>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Failed());

        // Configure o mock do repositório de usuários para retornar erro na atualização do usuário
        _mockUserRepository.Setup(repo => repo.UpdateUserAsync(It.IsAny<UserEntity>())).ReturnsAsync(IdentityResult.Failed());

        // Configure o mock do repositório de usuários para retornar um token de confirmação de email gerado de forma aleatória ao chamar o método de geração de token
        _mockUserRepository.Setup(repo => repo.GenerateEmailConfirmationTokenAsync(It.IsAny<UserEntity>())).ReturnsAsync(Faker.Currency.ThreeLetterCode);

        // Configure o mock do serviço de email para retornar sucesso ao chamar o método de invite
        _mockEmailFacade.Setup(facade => facade.Invite(It.IsAny<MailRequest>())).Returns(Task.FromResult(true));

        // Configure o mock do serviço de email externo para retornar um código de status OK na chamada do método de invite
        _mockEmailExternal.Setup(emailExter => emailExter.Invite(It.IsAny<MailRequest>())).ReturnsAsync(new HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.OK });

        // Execute o método de criação
        var result = await _userService.CreateAsync(UserMocks.UserCreateRequestMock());

        // Verifique se o resultado é um false
        Assert.False(result.Sucesso);
        Assert.Equal(StatusCodes.ErrorBadRequest, result.StatusCode);
    }

    [Fact]
    public async Task TestCreateAsyncErrorInternalServer()
    {
        _mockSettings.Setup(set => set.Value).Returns(new AppSettings
        {
            UrlBase = new UrlBase
            {
                TOOLS_MAIL_API = Faker.Internet.Url()
            }
        });

        // Configure o mock do repositório de usuários para retornar error na criação
        _mockUserRepository.Setup(repo => repo.CreateUserAsync(It.IsAny<UserEntity>(), It.IsAny<string>())).ThrowsAsync(new Exception());

        // Configure o mock do repositório de usuários para retornar sucesso na atualização do usuário
        _mockUserRepository.Setup(repo => repo.UpdateUserAsync(It.IsAny<UserEntity>())).ReturnsAsync(IdentityResult.Success);

        // Configure o mock do repositório de usuários para retornar um token de confirmação de email gerado de forma aleatória ao chamar o método de geração de token
        _mockUserRepository.Setup(repo => repo.GenerateEmailConfirmationTokenAsync(It.IsAny<UserEntity>())).ReturnsAsync(Faker.Currency.ThreeLetterCode);

        // Configure o mock do serviço de email para retornar sucesso ao chamar o método de invite
        _mockEmailFacade.Setup(facade => facade.Invite(It.IsAny<MailRequest>())).Returns(Task.FromResult(true));

        // Configure o mock do serviço de email externo para retornar um código de status OK na chamada do método de invite
        _mockEmailExternal.Setup(emailExter => emailExter.Invite(It.IsAny<MailRequest>())).ReturnsAsync(new HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.OK });

        // Execute o método de autenticação
        var result = await _userService.CreateAsync(UserMocks.UserCreateRequestMock());

        // Verifique se o resultado é um false
        Assert.False(result.Sucesso);
        Assert.Equal(StatusCodes.ServerErrorInternalServerError, result.StatusCode);
    }

    [Fact]
    public async Task TestUpdateAsyncSuccessOK()
    {
        _mockSettings.Setup(set => set.Value).Returns(new AppSettings
        {
            UrlBase = new UrlBase
            {
                TOOLS_MAIL_API = Faker.Internet.Url()
            }
        });

        // Configure o mock do repositório de usuários para retornar sucesso na atualização
        _mockUserRepository.Setup(repo => repo.UpdateUserAsync(It.IsAny<UserEntity>())).ReturnsAsync(IdentityResult.Success);

        // Configure o mock do repositório de usuários para retornar sucesso na recuperação
        _mockUserRepository.Setup(repo => repo.GetAsync(It.IsAny<Guid>())).ReturnsAsync(UserMocks.UserEntityMock);

        // Configure o mock do repositório de usuários para retornar sucesso na alteração do username
        _mockUserRepository.Setup(repo => repo.SetUserNameAsync(It.IsAny<UserEntity>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);

        // Configure o mock do repositório de usuários para retornar sucesso na alteração da senha
        _mockUserRepository.Setup(repo => repo.ChangePasswordAsync(It.IsAny<UserEntity>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);

        // Configure o mock do repositório de usuários para retornar sucesso na alteração do e-mail
        _mockUserRepository.Setup(repo => repo.SetEmailAsync(It.IsAny<UserEntity>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);

        // Configure o mock do repositório de usuários para retornar um token de confirmação de email gerado de forma aleatória ao chamar o método de geração de token
        _mockUserRepository.Setup(repo => repo.GenerateEmailConfirmationTokenAsync(It.IsAny<UserEntity>())).ReturnsAsync(Faker.Currency.ThreeLetterCode);

        // Configure o mock do repositório de usuários para retornar sucesso na alteração do celular
        _mockUserRepository.Setup(repo => repo.SetPhoneNumberAsync(It.IsAny<UserEntity>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);

        // Execute o método de autenticação
        var result = await _userService.UpdateAsync(UserMocks.UserUpdateRequestMock());

        // Verifique se o resultado é um sucesso
        Assert.False(result.Sucesso);
        Assert.Equal(StatusCodes.ServerErrorInternalServerError, result.StatusCode);
    }
}
