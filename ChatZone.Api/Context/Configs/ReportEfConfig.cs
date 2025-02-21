using ChatZone.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatZone.Context.Configs;

public class ReportEfConfig : IEntityTypeConfiguration<Report>
{
    public void Configure(EntityTypeBuilder<Report> builder)
    {
        builder
            .HasKey(x => x.ReportId);

        builder
            .HasOne(x => x.PersonReporter)
            .WithMany(x => x.Reporter)
            .HasForeignKey(x => x.ReporterId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.PersonReported)
            .WithMany(x => x.Reported)
            .HasForeignKey(x => x.ReportedId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .Property(x => x.ReportTheme)
            .HasMaxLength(250)
            .IsRequired();
        
        builder.ToTable(nameof(Report));
    }
}