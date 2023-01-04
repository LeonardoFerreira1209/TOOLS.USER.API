using Hangfire.Annotations;
using Hangfire.Dashboard;

namespace APPLICATION.DOMAIN.UTILS.AUTH.CUSTOMAUTHORIZE.FILTER
{
    public class CustomAuthorizeHangfireFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize([NotNull] DashboardContext context)
        {
            //var httpContext = context.GetHttpContext();

            //// Allow all authenticated users to see the Dashboard (potentially dangerous).
            //return httpContext.Request.Query.Contains(new KeyValuePair<string, Microsoft.Extensions.Primitives.StringValues>("token", "token"));

            return true;
        }
    }
}
