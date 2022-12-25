using Serilog;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace APPLICATION.DOMAIN.UTILS;

/// <summary>
/// Tracker para chamadas de funções.
/// </summary>
[ExcludeFromCodeCoverage]
public static class Tracker
{
    public static async Task Time(Func<Task> method, string message)
    {
        var time = new Stopwatch();

        time.Start();

        await method();

        time.Stop();

        Log.Information($"[LOG INFORMATION] - {message}, Tempo: {time.Elapsed}\n");
    }

    public static async Task<T> Time<T>(Func<Task<T>> method, string message)
    {
        var time = new Stopwatch();

        time.Start();

        var result = await method();

        time.Stop();

        Log.Information($"[LOG INFORMATION] - {message}, Tempo: {time.Elapsed}\n");

        return result;
    }
}

