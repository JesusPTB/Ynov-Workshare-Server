using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Ynov_WorkShare_Server.Models;
using File = Ynov_WorkShare_Server.Models.File;

namespace Ynov_WorkShare_Server.Context;

public class WorkShareDbContext : IdentityDbContext<User>
{
    public WorkShareDbContext(DbContextOptions<WorkShareDbContext> options) : base(options)
    {
    }

    public DbSet<Channel> Channels { get; set; }
    public DbSet<UserChannel> UserChannels { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<File> Files { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasMany(u => u.AdministratedChannels).WithOne(c => c.Admin);

        modelBuilder.Entity<User>().HasMany(u => u.UserChannels).WithOne(uc => uc.User);

        modelBuilder.Entity<User>().HasMany(u => u.Messages).WithOne(m => m.Author);

        modelBuilder.Entity<User>().HasMany(u => u.File).WithOne(f => f.Author);


        modelBuilder.Entity<Channel>().HasMany(u => u.UserChannels).WithOne(uc => uc.Channel);

        modelBuilder.Entity<Channel>().HasMany(u => u.Messages).WithOne(m => m.Channel);

        modelBuilder.Entity<Channel>().HasMany(u => u.Files).WithOne(m => m.Channel);


        base.OnModelCreating(modelBuilder);
    }
}