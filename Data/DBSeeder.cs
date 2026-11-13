// Owner: Reyes | Type: static class (seed test accounts + sample data)
using GymApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GymApi.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext db, IPasswordHasher<User> hasher)
    {
        if (await db.Users.AnyAsync()) return;   // already seeded

        var admin = CreateUser(hasher, "admin", "Admin123!", "Gym Admin", Roles.Admin);
        var mia = CreateUser(hasher, "coach.mia", "Coach123!", "Coach Mia Santos", Roles.Instructor);
        var leo = CreateUser(hasher, "coach.leo", "Coach123!", "Coach Leo Reyes", Roles.Instructor);
        db.Users.AddRange(admin, mia, leo);

        var today = DateTime.Today;
        var juan = CreateMember(hasher, "juan", "Juan Dela Cruz", "juan@example.com", "Monthly", today.AddDays(20));
        var maria = CreateMember(hasher, "maria", "Maria Santos", "maria@example.com", "Monthly", today.AddDays(-10)); // OVERDUE
        var pedro = CreateMember(hasher, "pedro", "Pedro Reyes", "pedro@example.com", "Annual", today.AddDays(200));
        var ana = CreateMember(hasher, "ana", "Ana Lopez", "ana@example.com", "Quarterly", today.AddDays(60));
        db.Members.AddRange(juan, maria, pedro, ana);
        await db.SaveChangesAsync();

        var yoga = new GymClass { Name = "Morning Yoga", Description = "Gentle flow for all levels", InstructorId = mia.Id, StartTime = today.AddDays(1).AddHours(7), DurationMinutes = 60, Capacity = 10 };
        var hiit = new GymClass { Name = "HIIT Blast", Description = "High-intensity interval training", InstructorId = mia.Id, StartTime = today.AddDays(1).AddHours(18), DurationMinutes = 45, Capacity = 2 };
        var boxing = new GymClass { Name = "Boxing Basics", Description = "Footwork, jabs and combos", InstructorId = leo.Id, StartTime = today.AddDays(1).AddHours(18).AddMinutes(30), DurationMinutes = 45, Capacity = 12 };
        var zumba = new GymClass { Name = "Zumba Party", Description = "Dance cardio", InstructorId = leo.Id, StartTime = today.AddDays(2).AddHours(9), DurationMinutes = 60, Capacity = 15 };
        db.GymClasses.AddRange(yoga, hiit, boxing, zumba);
        await db.SaveChangesAsync();

        // HIIT Blast is already full (capacity 2) so the "class is full" rule can be demoed.
        db.Reservations.AddRange(
            new Reservation { MemberId = pedro.Id, GymClassId = hiit.Id, Status = ReservationStatus.Confirmed },
            new Reservation { MemberId = ana.Id, GymClassId = hiit.Id, Status = ReservationStatus.Reserved });
        await db.SaveChangesAsync();
    }

    private static User CreateUser(IPasswordHasher<User> hasher, string username, string password, string fullName, string role)
    {
        var user = new User { Username = username, FullName = fullName, Role = role };
        user.PasswordHash = hasher.HashPassword(user, password);
        return user;
    }

    private static Member CreateMember(IPasswordHasher<User> hasher, string username, string fullName, string email, string plan, DateTime expiry)
    {
        return new Member
        {
            User = CreateUser(hasher, username, "Member123!", fullName, Roles.Member),
            FullName = fullName,
            Email = email,
            MembershipPlan = plan,
            MembershipExpiry = expiry
        };
    }
}
