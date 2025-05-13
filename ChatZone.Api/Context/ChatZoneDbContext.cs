using ChatZone.Context.Configs;
using ChatZone.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatZone.Context;

public class ChatZoneDbContext : DbContext
{
    public DbSet<BlockedPerson> BlockedPeoples { get; set; }
    public DbSet<GroupMessage> GroupMessages { get; set; }
    public DbSet<GroupMember> ChatMembers { get; set; }
    public DbSet<SingleChat> SingleChats { get; set; }
    public DbSet<SingleMessage> SingleMessages { get; set; }
    public DbSet<GroupChat> GroupChats { get; set; }
    public DbSet<Person> Persons { get; set; }
    public DbSet<QuickMessage> QuickMessages { get; set; }
    public DbSet<Report> Reports { get; set; }
    public DbSet<Statistic> Statistics { get; set; }

    public ChatZoneDbContext(DbContextOptions<ChatZoneDbContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PersonEfConfig).Assembly);
    }
}