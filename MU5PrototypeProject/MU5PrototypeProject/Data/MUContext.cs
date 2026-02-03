using Microsoft.EntityFrameworkCore;
using MU5PrototypeProject.Models;

namespace MU5PrototypeProject.Data
{
    public class MUContext : DbContext
    {
        public MUContext(DbContextOptions<MUContext> options)
            : base(options)
        {
        }

        public DbSet<Trainer> Trainers { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<SessionExercise> SessionExercises { get; set; }
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<SessionNotes> SessionNotes { get; set; }
        public DbSet<AdminStatus> AdminStatuses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Trainer-Session (Restrict)
            modelBuilder.Entity<Trainer>()
               .HasMany(t => t.Sessions)
               .WithOne(s => s.Trainer)
               .HasForeignKey(s => s.TrainerID)
               .OnDelete(DeleteBehavior.Restrict);

            // Client-Session (Restrict)
            modelBuilder.Entity<Client>()
                .HasMany(c => c.Sessions)
                .WithOne(s => s.Client)
                .HasForeignKey(s => s.ClientID)
                .OnDelete(DeleteBehavior.Restrict);

            // Session-SessionExercise (1-M Cascade)
            modelBuilder.Entity<SessionExercise>()
                .HasOne(se => se.Session)
                .WithMany(s => s.Exercises)
                .HasForeignKey(se => se.SessionID)
                .OnDelete(DeleteBehavior.Cascade);

            // Exercise-SessionExercise (1-M Restrict)
            modelBuilder.Entity<SessionExercise>()
                .HasOne(se => se.Exercise)
                .WithMany(e => e.SessionExercises)
                .HasForeignKey(se => se.ExerciseID)
                .OnDelete(DeleteBehavior.Restrict);

            // Session-SessionNotes (1-1 Cascade) mapped via Session.Notes
            modelBuilder.Entity<Session>()
                .HasOne(s => s.Notes)
                .WithOne(n => n.Session)
                .HasForeignKey<SessionNotes>(n => n.SessionID)
                .OnDelete(DeleteBehavior.Cascade);

            // Session-AdminStatus (1-1 Cascade) mapped via Session.AdminStatus
            modelBuilder.Entity<Session>()
                .HasOne(s => s.AdminStatus)
                .WithOne(a => a.Session)
                .HasForeignKey<AdminStatus>(a => a.SessionID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}