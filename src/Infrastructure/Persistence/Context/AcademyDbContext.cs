﻿using Application.Common.Interfaces;
using Domain.Common;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Reflection;

namespace Infrastructure.Persistence.Context
{
	public class AcademyDbContext : DbContext, IAcademyDbContext
	{
		public AcademyDbContext(DbContextOptions<AcademyDbContext> options)
			: base(options)
		{
		}

		public DbSet<Test> Tests { get; set; }
		public DbSet<Domain.Entities.ExchangeRate> exchangeRates { get; set; }
		public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
		{
			foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
			{
				switch (entry.State)
				{
					case EntityState.Added:
						entry.Entity.CreatedBy = "unknown";
						entry.Entity.Created = DateTime.UtcNow;
						entry.Entity.LastModifiedBy = "unknown";
						entry.Entity.LastModified = DateTime.UtcNow;
						break;
					case EntityState.Modified:
						entry.Entity.LastModifiedBy = "unknown";
						entry.Entity.LastModified = DateTime.UtcNow;
						break;
				}
			}

			return base.SaveChangesAsync(cancellationToken);
		}

		protected override void OnConfiguring(DbContextOptionsBuilder options)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

			base.OnModelCreating(modelBuilder);
		}
	}
}
