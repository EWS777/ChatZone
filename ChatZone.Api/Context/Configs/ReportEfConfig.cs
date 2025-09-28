using ChatZone.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatZone.Context.Configs;

public class ReportEfConfig : IEntityTypeConfiguration<Report>
{
    public void Configure(EntityTypeBuilder<Report> builder)
    {
        builder
            .HasKey(x => x.IdReport);

        builder
            .HasOne(x => x.PersonReporter)
            .WithMany(x => x.Reporter)
            .HasForeignKey(x => x.IdReporter)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.PersonReported)
            .WithMany(x => x.Reported)
            .HasForeignKey(x => x.IdReported)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .Property(x => x.ReportTheme)
            .IsRequired();

        builder
            .Property(x => x.ReportMessage)
            .IsRequired()
            .HasMaxLength(1024);

        builder
            .HasOne(x => x.SingleMessage)
            .WithOne(x => x.Report)
            .HasForeignKey<Report>(x => x.IdSingleMessageReport)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder
            .HasOne(x => x.GroupMessage)
            .WithOne(x => x.Report)
            .HasForeignKey<Report>(x => x.IdGroupMessageReport)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.ToTable(nameof(Report));
    }
}