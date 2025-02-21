using ChatZone.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatZone.Context.Configs;

public class IndividualChatEfConfig : IEntityTypeConfiguration<IndividualChat>
{
    public void Configure(EntityTypeBuilder<IndividualChat> builder)
    {
        builder
            .HasOne(x => x.FirstPerson)
            .WithOne(x => x.FirstPerson)
            .HasForeignKey<IndividualChat>(x => x.FirstPersonId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.SecondPerson)
            .WithOne(x => x.SecondPerson)
            .HasForeignKey<IndividualChat>(x => x.SecondPersonId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.ToTable(nameof(IndividualChat));
    }
}