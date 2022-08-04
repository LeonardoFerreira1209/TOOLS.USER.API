namespace APPLICATION.DOMAIN.UTILS.GLOBAL;

public static class GlobalData<T> where T : class
{
    public static ICollection<KeyValuePair<string, T>> GlobalItems { get; set; } = new HashSet<KeyValuePair<string, T>>();
}
