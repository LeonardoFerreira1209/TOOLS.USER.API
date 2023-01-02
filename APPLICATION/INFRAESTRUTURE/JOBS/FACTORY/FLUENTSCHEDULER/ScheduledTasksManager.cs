using APPLICATION.INFRAESTRUTURE.JOBS.INTERFACES.BASE;
using FluentScheduler;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace APPLICATION.INFRAESTRUTURE.JOBS.FACTORY.FLUENTSCHEDULER;

[ExcludeFromCodeCoverage]
public class ScheduledTasksManager
{
    private readonly IServiceProvider _serviceProvider;

    public ScheduledTasksManager(IServiceProvider serviceProvider) { _serviceProvider = serviceProvider; }

    public void StartJobs()
    {
        Registry jobsRegistry = (Registry)_serviceProvider.GetService<IFluentSchedulerJobs>();

        JobManager.JobFactory = new FluentSchedulerJobFactory(_serviceProvider);

        JobManager.Initialize(jobsRegistry); JobManager.UseUtcTime(); JobManager.Start();
    }
}
