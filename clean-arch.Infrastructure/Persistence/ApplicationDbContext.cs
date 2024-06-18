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
        // modelBuilder.Entity<Employee>().Property(e => e.DisplayName)
        //     .HasComputedColumnSql("[FirstName]+' '+[LastName]", true);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        InterceptSaveChanges();
        return base.SaveChangesAsync(cancellationToken);
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        InterceptSaveChanges();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    private void InterceptSaveChanges()
    {
        foreach (var entry in ChangeTracker.Entries())
        {
            if (entry.Entity is BaseEntity entity)
                switch (entry.State)
                {
                    case EntityState.Added:
                        entity.CreatedAt = DateTime.Now.ToUniversalTime();
                        entity.CreatedBy = "SYSTEM";
                        break;
                    case EntityState.Modified:
                        entity.UpdatedAt = DateTime.Now.ToUniversalTime();
                        entity.UpdatedBy = "SYSTEM";
                        break;
                    case EntityState.Deleted:
                        entity.DeletedAt = DateTime.Now.ToUniversalTime();
                        entity.DeletedBy = "SYSTEM";
                        break;
                }
        }
    }
}