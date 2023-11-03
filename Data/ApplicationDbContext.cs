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
    }
    public DbSet<User> Users { get; set; }
    public DbSet<Tasks> Tasks { get; set; }
    public DbSet<Holiday> Holidays { get; set; }
    public DbSet<HolidayType> HolidayTypes { get; set; }
    public DbSet<Type> Types { get; set; }
}