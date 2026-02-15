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
            if (_httpContextAccessor.HttpContext != null)
            {
                //We have a HttpContext, but there might not be anyone Authenticated
                UserName = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "Unknown";
            }
            else
            {
                //No HttpContext so seeding data
                UserName = "Seed Data";
            }
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
        public DbSet<SessionNotes> SessionNotes { get; set; }
        public DbSet<AdminStatus> AdminStatuses { get; set; }
        public DbSet<Equipment> Equipments { get; set; }

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

            // Session-SessionNotes (1-1 Cascade)
            modelBuilder.Entity<Session>()
                .HasOne(s => s.Notes)
                .WithOne(n => n.Session)
                .HasForeignKey<SessionNotes>(n => n.SessionID)
                .OnDelete(DeleteBehavior.Cascade);

            // Session-AdminStatus (1-1 Cascade)
            modelBuilder.Entity<Session>()
                .HasOne(s => s.AdminStatus)
                .WithOne(a => a.Session)
                .HasForeignKey<AdminStatus>(a => a.SessionID)
                .OnDelete(DeleteBehavior.Cascade);

            // Session-Equipment (1-1 Cascade)
            modelBuilder.Entity<Session>()
                .HasOne(s => s.Equipment)
                .WithOne(e => e.Session)
                .HasForeignKey<Equipment>(e => e.SessionID)
                .OnDelete(DeleteBehavior.Cascade);
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            OnBeforeSaving();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }
        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
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