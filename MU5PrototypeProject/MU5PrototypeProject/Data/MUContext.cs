using Microsoft.EntityFrameworkCore;
using MU5PrototypeProject.Models;

namespace MU5PrototypeProject.Data
{
    public class MUContext : DbContext
    {
        //To give access to IHttpContextAccessor for Audit Data with IAuditable
        private readonly IHttpContextAccessor _httpContextAccessor;

        //Property to hold the UserName value
        public string UserName
        {
            get; private set;
        }

        public MUContext(DbContextOptions<MUContext> options, IHttpContextAccessor httpContextAccessor)
             : base(options)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            UserName = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "Unknown";
        }

        public MUContext(DbContextOptions<MUContext> options)
            : base(options)
        {
            _httpContextAccessor = null!;
            UserName = "Seed Data";
        }

        public DbSet<Trainer> Trainers { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<SessionExercise> SessionExercises { get; set; }
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<Apparatus> Apparatuses { get; set; }
        public DbSet<ExerciseSettings> ExerciseSettings { get; set; }
        public DbSet<SessionNotes> SessionNotes { get; set; }
        public DbSet<AdminStatus> AdminStatuses { get; set; }
        public DbSet<PhysioInfo> PhysioInfos { get; set; }
        public DbSet<Prop> Props { get; set; }
        public DbSet<ExerciseProp> ExerciseProps { get; set; }

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

            // Session-SessionExercise (Cascade)
            modelBuilder.Entity<SessionExercise>()
                .HasOne(se => se.Session)
                .WithMany(s => s.Exercises)
                .HasForeignKey(se => se.SessionID)
                .OnDelete(DeleteBehavior.Cascade);

            // Exercise-SessionExercise (Restrict)
            modelBuilder.Entity<SessionExercise>()
                .HasOne(se => se.Exercise)
                .WithMany(e => e.SessionExercises)
                .HasForeignKey(se => se.ExerciseID)
                .OnDelete(DeleteBehavior.Restrict);

            // Apparatus-Exercise (Restrict)
            modelBuilder.Entity<Exercise>()
                .HasOne(e => e.Apparatus)
                .WithMany(a => a.Exercises)
                .HasForeignKey(e => e.ApparatusID)
                .OnDelete(DeleteBehavior.Restrict);

            // Exercise-ExerciseSettings (Cascade)
            modelBuilder.Entity<ExerciseSettings>()
                .HasOne(es => es.Exercise)
                .WithMany()
                .HasForeignKey(es => es.ExerciseID)
                .OnDelete(DeleteBehavior.Cascade);

            // Session-SessionNotes (1-1 Cascade)
            modelBuilder.Entity<Session>()
                .HasOne(s => s.Notes)
                .WithOne(n => n.Session)
                .HasForeignKey<SessionNotes>(n => n.SessionID)
                .OnDelete(DeleteBehavior.Cascade);

            // SessionNotes-Trainer (Restrict, nullable)
            modelBuilder.Entity<SessionNotes>()
                .HasOne(n => n.CompletedByTrainer)
                .WithMany()
                .HasForeignKey(n => n.CompletedByTrainerID)
                .OnDelete(DeleteBehavior.Restrict);

            // Session-AdminStatus (1-1 Cascade)
            modelBuilder.Entity<Session>()
                .HasOne(s => s.AdminStatus)
                .WithOne(a => a.Session)
                .HasForeignKey<AdminStatus>(a => a.SessionID)
                .OnDelete(DeleteBehavior.Cascade);

            // Session-PhysioInfo (1-1 Cascade)
            modelBuilder.Entity<Session>()
                .HasOne(s => s.PhysioInfo)
                .WithOne(p => p.Session)
                .HasForeignKey<PhysioInfo>(p => p.SessionID)
                .OnDelete(DeleteBehavior.Cascade);
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            OnBeforeSaving();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default)
        {
            OnBeforeSaving();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void OnBeforeSaving()
        {
            var entries = ChangeTracker.Entries();
            foreach (var entry in entries)
            {
                if (entry.Entity is IAuditable trackable)
                {
                    var now = DateTime.UtcNow;
                    switch (entry.State)
                    {
                        case EntityState.Modified:
                            trackable.UpdatedOn = now;
                            trackable.UpdatedBy = UserName;
                            break;
                        case EntityState.Added:
                            trackable.CreatedOn = now;
                            trackable.CreatedBy = UserName;
                            trackable.UpdatedOn = now;
                            trackable.UpdatedBy = UserName;
                            break;
                    }
                }
            }
        }
    }
}