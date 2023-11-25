using Hangfire;
using Scheduling.Jobs;

namespace Scheduling.Services
{
    public class SchedulingService: ISchedulingService
    {
        #region Fields
        private readonly ScheduleJob scheduleJob;
        #endregion

        #region Constructor
        public SchedulingService(ScheduleJob scheduleJob)
        {
            this.scheduleJob = scheduleJob;
        }
        #endregion

        #region ISchedulingService
        public bool GetExchangeRate(DateTime date, CancellationToken cancellationToken)
        {
            try
            {
                RecurringJob.AddOrUpdate("Exchange Rate", () => scheduleJob.GetExchangeRateJob(date, cancellationToken), Cron.Minutely);

                //BackgroundJob.Enqueue(() => scheduleJob.GetExchangeRateJob(date, cancellationToken));

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                return false;
            }
        }

        #endregion
    }
}
