using ChatZone.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatZone.Context.Configs;

public class SingleChatEfConfig : IEntityTypeConfiguration<SingleChat>
{
    public void Configure(EntityTypeBuilder<SingleChat> builder)
    {
        builder
            .HasKey(x => x.IdSingleChat);

        builder
            .HasOne(x => x.FirstPerson)
            .WithOne(x => x.FirstPerson)
            .HasForeignKey<SingleChat>(x => x.IdFirstPerson)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder
            .HasOne(x => x.SecondPerson)
            .WithOne(x => x.SecondPerson)
            .HasForeignKey<SingleChat>(x => x.IdSecondPerson)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .Property(x => x.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()");
        
        
        
        builder.ToTable(nameof(SingleChat));
    }
}