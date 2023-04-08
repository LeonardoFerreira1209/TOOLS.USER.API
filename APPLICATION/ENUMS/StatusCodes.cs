using System.ComponentModel;

namespace APPLICATION.ENUMS;

public enum StatusCodes
{
    #region Informal codes
    [Description("Continue")]
    InformalContinue = 100,

    [Description("Switching Protocols")]
    InformalSwitchingProtocols = 101,

    [Description("Processing")]
    InformalProcessing = 102,
    #endregion

    #region Success codes
    [Description("Ok")]
    SuccessOK = 200,

    [Description("Created")]
    SuccessCreated = 201,

    [Description("Accepted")]
    SuccessAccepted = 202,

    [Description("Non-authoritative Information")]
    SuccessNonAuthoritativeInformation = 203,

    [Description("No Content")]
    SuccessNoContent = 204,

    [Description("Reset Content")]
    SuccessResetContent = 205,

    [Description("Partial Content")]
    SuccessPartialContent = 206,

    [Description("Multi-Status")]
    SuccessMultiStatus = 207,

    [Description("Altead Reported")]
    SuccessAlreadReported = 208,

    [Description("Im used")]
    SuccessImUsed = 226,
    #endregion

    #region Redirection codes
    [Description("Multiple Choices")]
    RedirectionMultipleChoices = 300,

    [Description("Moved Permanently")]
    RedirectionMovedPermanently = 301,

    [Description("Found")]
    RedirectionFound = 302,

    [Description("See Other")]
    RedirectionSeeOther = 303,

    [Description("Not Modified")]
    RedirectionNotModified = 304,

    [Description("Use Proxy")]
    RedirectionUseProxy = 305,

    [Description("Temporary Redirect")]
    RedirectionTemporaryRedirect = 307,

    [Description("Permanent Redirect")]
    RedirectionPermanetRedirect = 308,
    #endregion

    #region Client error codes
    [Description("Bad Request")]
    ErrorBadRequest = 400,

    [Description("Unauthorized")]
    ErrorUnauthorized = 401,

    [Description("Payment Required")]
    ErrorPaymentRequired = 402,

    [Description("Forbidden")]
    ErrorForbidden = 403,

    [Description("Not Found")]
    ErrorNotFound = 404,

    [Description("Method Not Allowed")]
    ErrorMethodNotAllowed = 405,

    [Description("Not Acceptable")]
    ErrorNotAcceptable = 406,

    [Description("Proxy Authentication Required")]
    ErrorProxyAuthenticationRequired = 407,

    [Description("Request Timeout")]
    ErrorRequestTimeout = 408,

    [Description("Conflict")]
    ErrorConflict = 409,

    [Description("Gone")]
    ErrorGone = 410,

    [Description("Length Required")]
    ErrorLengthRequired = 411,

    [Description("Precondition Failed")]
    ErrorPreconditionFailed = 412,

    [Description("Payload Too Large")]
    ErrorPayloadTooLarge = 413,

    [Description("Request-URI Too Long")]
    ErrorRequestURITooLong = 414,

    [Description("Unsupoprted Media Type")]
    ErrorUnsupportedMediaType = 415,

    [Description("Requested Range Not Satisfiable")]
    ErrorRequestedRangeNotSatisfiable = 416,

    [Description("Expectation Failed")]
    ErrorExpectationFailed = 417,

    [Description("I'm a Teapot")]
    ErrorImATeapot = 418,

    [Description("Misdirected Request")]
    ErrorMisdirectedRequest = 421,

    [Description("Unprocessable Entity")]
    ErrorUnprocessableEntity = 422,

    [Description("Locked")]
    ErrorLocked = 423,

    [Description("Failed Dependency")]
    ErrorFailedDependency = 424,

    [Description("Upgrade Required")]
    ErrorUpgradeRequired = 426,

    [Description("Precondition Required")]
    ErrorPrecondintionRequired = 428,

    [Description("Too Many Requests")]
    ErrorTooManyRequest = 429,

    [Description("Request Header Fields Too Large")]
    ErrorRequestHeaderFieldsTooLarge = 431,

    [Description("Connection Closed Without Response")]
    ErrorConnectionClosedWithoutResponse = 444,

    [Description("Unavailable For Legal Reasons")]
    ErrorUnavailableForLegalReasons = 451,

    [Description("Client Closed Request")]
    ErrorClientClosedRequest = 499,
    #endregion

    #region Server error codes
    [Description("Internal Server Error")]
    ServerErrorInternalServerError = 500,

    [Description("Not Implemented")]
    ServerErrorNotImplemented = 501,

    [Description("Bad Gateway")]
    ServerErrorBadGateway = 502,

    [Description("Service Unavailable")]
    ServerErrorServiceUnavailable = 503,

    [Description("Gateway Timeout")]
    ServerErrorGatewayTimeout = 504,

    [Description("HTTP Version Not Supported")]
    ServerErrorHTTPVersionNotSupported = 505,

    [Description("Variant Also Negotiates")]
    ServerErrorVariantAlsoNegotiates = 506,

    [Description("Insufficient Storage")]
    ServerErrorInsufficientStorage = 507,

    [Description("Loop Detected")]
    ServerErrorLoopDetected = 508,

    [Description("Not Extended")]
    ServerErrorNotExtended = 510,

    [Description("Network Authentication Required")]
    ServerErrorNetworkAuthenticationRequired = 511,

    [Description("Network Connect Timeout Error")]
    ServerErrorNetworkConnectTimeoutError = 599,
    #endregion
}
