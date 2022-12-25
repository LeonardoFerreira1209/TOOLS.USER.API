using APPLICATION.DOMAIN.DTOS.REQUEST.USER;
using APPLICATION.DOMAIN.VALIDATORS;
using Moq;
using TOOLS.USER.API.TEST.MOCKS.USER;
using Xunit;

namespace TOOLS.USER.API.TEST.VALIDATORS
{
    public class CreateUserValidatorMock
    {
        public readonly CreateUserValidator _createUserValidatorMock;

        public CreateUserValidatorMock()
        {
            _createUserValidatorMock= new CreateUserValidator();
        }

        [Fact]
        public async Task ValidPayload()
        {
            var validationResult =  await _createUserValidatorMock.ValidateAsync(UserMocks.UserCreateRequestMock());

            Assert.True(validationResult.IsValid);
        }
    }
}
