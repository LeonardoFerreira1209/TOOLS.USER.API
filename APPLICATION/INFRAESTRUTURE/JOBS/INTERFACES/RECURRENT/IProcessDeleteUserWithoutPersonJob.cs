using FluentScheduler;

namespace APPLICATION.INFRAESTRUTURE.JOBS.INTERFACES.RECURRENT;

public interface IProcessDeleteUserWithoutPersonJob : IJob
{
    Task DeleteUserWithoutPerson();
}
