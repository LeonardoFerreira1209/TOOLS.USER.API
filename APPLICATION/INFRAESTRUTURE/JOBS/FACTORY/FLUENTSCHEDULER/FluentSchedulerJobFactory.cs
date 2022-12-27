using FluentScheduler;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace APPLICATION.INFRAESTRUTURE.JOBS.FACTORY.FLUENTSCHEDULER;

[ExcludeFromCodeCoverage]
public sealed class FluentSchedulerJobFactory : IJobFactory
{
    private readonly IServiceProvider _serviceProvider;

    public FluentSchedulerJobFactory(IServiceProvider serviceProvider) { _serviceProvider = serviceProvider; }

    public IJob GetJobInstance<T>() where T : IJob => _serviceProvider.GetService<T>();
}
