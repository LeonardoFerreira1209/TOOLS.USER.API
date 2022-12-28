using APPLICATION.DOMAIN.DTOS.REQUEST.USER;
using APPLICATION.DOMAIN.ENTITY.USER;
using APPLICATION.ENUMS;

namespace TOOLS.USER.API.TEST.MOCKS.USER;

public static class UserMocks
{
    public static LoginRequest LoginRequestMock() => new(Faker.Internet.UserName(), Faker.Internet.SecureUrl());

    public static UserCreateRequest UserCreateRequestMock() => new()
    {
        BirthDay = "12/09/1999",
        PlanId = Guid.NewGuid(),
        CPF = "47841467842",
        Email= Faker.Internet.Email(),
        FirstName = Faker.Name.First(),
        LastName = Faker.Name.Last(),
        Gender = Faker.Enum.Random<Gender>(),
        ImageUri = Faker.Internet.Url(),
        Password = Faker.Internet.SecureUrl(),
        PhoneNumber = Faker.Phone.Number(),
        RG = "446997882",
        UserName = Faker.Internet.UserName()
    };

    public static UserUpdateRequest UserUpdateRequestMock() => new()
    {
        Id= Guid.NewGuid(),
        Age = Faker.RandomNumber.Next(18, 99),
        BirthDay = "12/09/1999",
        CPF = "47841467842",
        Email = Faker.Internet.Email(),
        FirstName = Faker.Name.First(),
        LastName = Faker.Name.Last(),
        Gender = Faker.Enum.Random<Gender>(),
        Password = Faker.Internet.SecureUrl(),
        PhoneNumber = Faker.Phone.Number(),
        RG = "446997882",
        UserName = Faker.Internet.UserName()
    };

    public static UserEntity UserEntityMock() => new()
    {
        Age = Faker.RandomNumber.Next(18, 99),
        BirthDay = "12/09/1999",
        PlanId = Guid.NewGuid(),
        CPF = "47841467842",
        Email = Faker.Internet.Email(),
        FirstName = Faker.Name.First(),
        LastName = Faker.Name.Last(),
        Gender = Faker.Enum.Random<Gender>(),
        ImageUri = Faker.Internet.Url(),
        PhoneNumber = Faker.Phone.Number(),
        RG = "446997882",
        UserName = Faker.Internet.UserName(),
    };

    public static ActivateUserRequest ActivateUserRequestMock() => new("code-activate", Guid.NewGuid());
}