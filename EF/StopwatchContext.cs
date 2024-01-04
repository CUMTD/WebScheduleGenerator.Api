using Microsoft.EntityFrameworkCore;
using WebScheduleGenerator.Core.Entities.Stopwatch;
using WebScheduleGenerator.EF.Configuration;

namespace WebScheduleGenerator.EF
{
	public class StopwatchContext(DbContextOptions<StopwatchContext> options) : DbContext(options)
	{
		public DbSet<Stop> Stops { get; protected set; }
		public DbSet<ParentStop> ParentStops { get; protected set; }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			_ = builder.ApplyConfiguration(new ParentStopConfiguration());
			_ = builder.ApplyConfiguration(new StopConfiguration());
		}

	}
}
