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
        
        builder.ToTable(nameof(MatchQueue));
    }
}