using Application.Common.Interfaces;
using Domain.Entities;
using Scheduling.Common.Entities;
using System.Text.Json;

namespace Scheduling.Jobs
{
    public class ScheduleJob/*: IScheduleJob*/
    {
        #region Fields
        private readonly HttpClient httpClient = new();
        //private readonly IAcademyDbContext context;
        private readonly IServiceProvider serviceProvider;

        private DateTime StartDate;
        #endregion

        #region Constructor
        public ScheduleJob(/*IAcademyDbContext academyDbContext*/ IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            //this.context = academyDbContext;
        }
        #endregion

        public async Task<bool> GetExchangeRateJob(DateTime date, CancellationToken cancellationToken)
        {
            return await GetExchangeRateFromApi(date, cancellationToken);
        }

        private async Task<bool> GetExchangeRateFromApi(DateTime date, CancellationToken cancellationToken)
        {
            try
            {
                StartDate = StartDate == DateTime.MinValue ? date : StartDate;

                if (StartDate.Date > DateTime.Now.Date)
                {
                    return true;
                }

                var req = new HttpRequestMessage()
                {
                    RequestUri = new Uri($"https://api.hnb.hr/tecajn-eur/v3?datum-primjene={StartDate.Date.ToString("yyyy-MM-dd")}"),
                    Method = HttpMethod.Get,
                };

                HttpResponseMessage response = await httpClient.SendAsync(req);
                var responseBody = await response.Content.ReadAsStringAsync();

                var exchangeRate = JsonSerializer.Deserialize<List<ExchangeRateViewModel>>(responseBody);
                
                var list = exchangeRate.Select(x => new ExchangeRate()
                {
                    Id = Guid.NewGuid(),
                    ImeValute = x.valuta,
                    KupovniTecaj = Decimal.Parse(x.kupovni_tecaj),
                    SrednjiTecaj = Decimal.Parse(x.srednji_tecaj),
                    Prodajni = Decimal.Parse(x.prodajni_tecaj),
                    DatumUnosa = DateTime.Parse(x.datum_primjene)
                }).ToList();

                if (list.Any())
                {
                    await SaveExchangeRates(list, cancellationToken);
                }
                
                StartDate = StartDate.AddDays(1);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                throw;
            }
        }

        private async Task SaveExchangeRates(List<ExchangeRate> exchangeRates, CancellationToken cancellationToken)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<IAcademyDbContext>();
                await context.exchangeRates.AddRangeAsync(exchangeRates);
                await context.SaveChangesAsync(cancellationToken);
            }
            //await context.exchangeRates.AddRangeAsync(exchangeRates);
            //await context.SaveChangesAsync(cancellationToken);
        }
    }
}
