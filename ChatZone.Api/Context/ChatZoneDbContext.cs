using ChatZone.Context.Configs;
using ChatZone.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatZone.Context;

public class ChatZoneDbContext : DbContext
{
    public DbSet<BlockedPeople> BlockedPeoples { get; set; }
    public DbSet<Chat> Chats { get; set; }
    public DbSet<ChatMember> ChatMembers { get; set; }
    public DbSet<IndividualChat> IndividualChats { get; set; }
    public DbSet<GroupChat> GroupChats { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<PersonData> PersonDatas { get; set; }
    public DbSet<Person> Persons { get; set; }
    public DbSet<PersonSettings> PersonSettings { get; set; }
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
        
        modelBuilder.Entity<Chat>().UseTptMappingStrategy();
    }
}