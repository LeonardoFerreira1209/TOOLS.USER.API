using APPLICATION.DOMAIN.DTOS.USER;

namespace APPLICATION.DOMAIN.UTILS.GLOBAL;

public static class GlobalData<T> where T : class
{
    public static UserData GlobalUser { get; set; }
}
