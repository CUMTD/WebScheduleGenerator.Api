using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebScheduleGenerator.Core.Entities.Stopwatch;

namespace WebScheduleGenerator.EF.Configuration
{
	internal class ParentStopConfiguration : IEntityTypeConfiguration<ParentStop>
	{
		public void Configure(EntityTypeBuilder<ParentStop> builder) { }
	}
}
