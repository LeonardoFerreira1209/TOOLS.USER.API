using APPLICATION.DOMAIN.VALIDATORS;
using TOOLS.USER.API.TEST.MOCKS.USER;
using Xunit;

namespace TOOLS.USER.API.TEST.VALIDATORS
{
    public class AuthenticationValidatorMock
    {
        public readonly AuthenticationValidator _authenticationValidator;

        public AuthenticationValidatorMock()
        {
            _authenticationValidator= new AuthenticationValidator();
        }

        [Fact]
        public async Task ValidPayload()
        {
            var validationResult =  await _authenticationValidator.ValidateAsync(UserMocks.LoginRequestMock());

            Assert.True(validationResult.IsValid);
        }
    }
}
