using ChatZone.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatZone.Shared.Context.Configs;

public class MatchQueueEfConfig : IEntityTypeConfiguration<MatchQueue>
{
    public void Configure(EntityTypeBuilder<MatchQueue> builder)
    {
        builder.HasKey(x => x.IdPerson);

        builder
            .Property(x => x.IdPerson)
            .ValueGeneratedNever();

        builder
            .Property(x => x.YourGender)
            .IsRequired();
        
        builder
            .Property(x => x.PartnerGender)
            .IsRequired();

        builder
            .Property(x => x.Language)
            .IsRequired();
        
        builder
            .Property(x => x.JoinedAt)
            .IsRequired();
        
        builder.ToTable(nameof(MatchQueue));
    }
}