using clean_arch.Domain.Entities;
using clean_arch.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace clean_arch.Infrastructure.Persistence;

internal class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    internal DbSet<Employee> Employees { get; set; }

    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    // {
    //     // base.OnConfiguring(optionsBuilder);
    //     optionsBuilder.UseNpgsql("Username=root;Password=password;Host=localhost;Database=clean_arch");
    // }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // register enums
        modelBuilder.HasPostgresEnum<Gender>();

        //other settings
        modelBuilder.Entity<Employee>().ToTable("employee_tbl");
    }
}