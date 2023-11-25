namespace Scheduling.Jobs
{
    public interface IScheduleJob
    {
        Task<bool> GetExchangeRateJob(DateTime date, CancellationToken cancellationToken);
    }
}
