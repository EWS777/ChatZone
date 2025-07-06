using ChatZone.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatZone.Context.Configs;

public class MatchQueueEfConfig : IEntityTypeConfiguration<MatchQueue>
{
    public void Configure(EntityTypeBuilder<MatchQueue> builder)
    {
        builder.HasKey(x => x.IdPerson);
        
        builder.ToTable(nameof(MatchQueue));
    }
}