using FluentScheduler;

namespace APPLICATION.INFRAESTRUTURE.JOBS.INTERFACES;

public interface IProcessDeleteUserWithoutPersonJob : IJob
{
    Task DeleteUserWithoutPerson();
}
