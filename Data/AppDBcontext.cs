// Owner: Reyes | Type: class (EF Core DbContext)
using GymApi.Models;
using Microsoft.EntityFrameworkCore;

namespace GymApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Member> Members => Set<Member>();
    public DbSet<GymClass> GymClasses => Set<GymClass>();
    public DbSet<Reservation> Reservations => Set<Reservation>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasIndex(u => u.Username).IsUnique();
        });

        // All relationships use Restrict: nothing is hard-deleted (soft delete / status instead),
        // and it avoids SQL Server "multiple cascade paths" errors.
        modelBuilder.Entity<Member>(entity =>
        {
            entity.HasIndex(m => m.UserId).IsUnique();
            entity.HasOne(m => m.User).WithMany().HasForeignKey(m => m.UserId).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<GymClass>(entity =>
        {
            entity.HasOne(c => c.Instructor).WithMany().HasForeignKey(c => c.InstructorId).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Reservation>(entity =>
        {
            entity.HasIndex(r => new { r.MemberId, r.GymClassId });
            entity.HasOne(r => r.Member).WithMany().HasForeignKey(r => r.MemberId).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(r => r.GymClass).WithMany().HasForeignKey(r => r.GymClassId).OnDelete(DeleteBehavior.Restrict);
        });
    }
}
