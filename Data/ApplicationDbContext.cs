// File Path: ./Data/ApplicationDbContext.cs
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<HolidayType>()
            .HasKey(ht => new { ht.HolidayId, ht.TypeId });

        modelBuilder.Entity<Tags>()
            .HasKey(t => new { t.Id, t.Username });

        modelBuilder.Entity<TaskTags>()
            .HasKey(tt => new { tt.TaskId, tt.TagId, tt.Username });
    }
    public DbSet<User> Users { get; set; }
    public DbSet<Tasks> Tasks { get; set; }
    public DbSet<Holiday> Holidays { get; set; }
    public DbSet<HolidayType> HolidayTypes { get; set; }
    public DbSet<Type> Types { get; set; }
    public DbSet<Tags> Tags { get; set; }
    public DbSet<TaskTags> TaskTags { get; set; }
}