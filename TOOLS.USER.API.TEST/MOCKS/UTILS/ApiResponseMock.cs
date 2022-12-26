using APPLICATION.DOMAIN.DTOS.RESPONSE.FILE;
using APPLICATION.DOMAIN.DTOS.RESPONSE.UTILS;
using APPLICATION.DOMAIN.ENUM;

namespace TOOLS.USER.API.TEST.MOCKS.UTILS;

public static class ApiResponseMock
{
    public static ApiResponse<object> ApiResponseFileResponseSuccessOkMock() => new(true, StatusCodes.SuccessOK, new FileResponse { FileUri = Faker.Internet.Url() }, null);

    public static ApiResponse<object> ApiResponseFileResponseServerErrorInternalServerErrorMock() => new(false, StatusCodes.ServerErrorInternalServerError, new FileResponse { FileUri = Faker.Internet.Url() }, null);
}
