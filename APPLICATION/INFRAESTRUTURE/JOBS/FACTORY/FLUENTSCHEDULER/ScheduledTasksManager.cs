using APPLICATION.INFRAESTRUTURE.JOBS.INTERFACES;
using FluentScheduler;
using Microsoft.Extensions.DependencyInjection;

namespace APPLICATION.INFRAESTRUTURE.JOBS.FACTORY.FLUENTSCHEDULER;

public class ScheduledTasksManager
{
    private readonly IServiceProvider _serviceProvider;

    public ScheduledTasksManager(IServiceProvider serviceProvider) { _serviceProvider = serviceProvider; }

    public void StartJobs()
    {
        Registry jobsRegistry = (Registry)_serviceProvider.GetService<IRegistryJobs>();

        JobManager.JobFactory = new FluentSchedulerJobFactory(_serviceProvider);

        JobManager.Initialize(jobsRegistry); JobManager.UseUtcTime(); JobManager.Start();
    }
}
