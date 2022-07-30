using FluentScheduler;
using Microsoft.Extensions.DependencyInjection;

namespace APPLICATION.INFRAESTRUTURE.JOBS.FACTORY.FLUENTSCHEDULER;

public sealed class FluentSchedulerJobFactory : IJobFactory
{
    private readonly IServiceProvider _serviceProvider;

    public FluentSchedulerJobFactory(IServiceProvider serviceProvider) { _serviceProvider = serviceProvider; }

    public IJob GetJobInstance<T>() where T : IJob => _serviceProvider.GetService<T>();
}
