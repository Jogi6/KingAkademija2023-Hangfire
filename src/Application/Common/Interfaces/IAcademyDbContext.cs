using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
	public interface IAcademyDbContext
	{
		public DbSet<Test> Tests { get; set; }
        public DbSet<Domain.Entities.ExchangeRate> exchangeRates { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
	}
}
