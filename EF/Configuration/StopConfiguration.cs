using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebScheduleGenerator.Core.Entities.Stopwatch;

namespace WebScheduleGenerator.EF.Configuration
{
	internal class StopConfiguration : IEntityTypeConfiguration<Stop>
	{
		public void Configure(EntityTypeBuilder<Stop> builder)
		{
			_ = builder
				.ToTable("Stop", "transit");

			_ = builder
				.HasKey(t => t.Id);

			_ = builder
				.Property(t => t.Id)
				.HasColumnName("Id")
				.HasMaxLength(50)
				.IsRequired();

			_ = builder
				.Property(t => t.Name)
				.HasColumnName("Name")
				.IsRequired();

			_ = builder
				.Property(t => t.Latitude)
				.HasColumnName("Latitude")
				.IsRequired();

			_ = builder
				.Property(t => t.Longitude)
				.HasColumnName("Longitude")
				.IsRequired();

			_ = builder
				.Property(t => t.Description)
				.HasColumnName("Description")
				.IsRequired(false);

			_ = builder
				.Property(t => t.Url)
				.HasColumnName("Url")
				.IsRequired(false);

			_ = builder
				.Property(t => t.Timezone)
				.HasColumnName("Timezone")
				.IsRequired();

			_ = builder
				.Property(t => t.Accessible)
				.HasColumnName("Accessible")
				.IsRequired();

			_ = builder
				.Property(t => t.Active)
				.HasColumnName("Active")
				.IsRequired();

			_ = builder
				.HasDiscriminator<byte>("Discriminator")
				.HasValue<ParentStop>(1);
		}

	}
}
