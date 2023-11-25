namespace Scheduling.Services
{
    public interface ISchedulingService
    {
        bool GetExchangeRate(DateTime date, CancellationToken cancellationToken);
    }
}
