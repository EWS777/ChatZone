using ChatZone.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatZone.Context.Configs;

public class StatisticEfConfig : IEntityTypeConfiguration<Statistic>
{
    public void Configure(EntityTypeBuilder<Statistic> builder)
    {
        builder
            .HasKey(x => x.StatisticId);

        builder
            .Property(x => x.Day)
            .IsRequired();

        builder
            .Property(x => x.AllMember)
            .IsRequired();
        
        builder
            .Property(x => x.OnlineNow)
            .IsRequired();
        
        builder
            .Property(x => x.OnlineIndividual)
            .IsRequired();
        
        builder
            .Property(x => x.OnlineGroup)
            .IsRequired();
        
        builder
            .Property(x => x.OnlineToday)
            .IsRequired();
        
        builder.ToTable(nameof(Statistic));
    }
}