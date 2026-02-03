using Microsoft.EntityFrameworkCore;
using MU5PrototypeProject.Models;
using System.Numerics;

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


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Trainer-Session relationship
            modelBuilder.Entity<Trainer>()
               .HasMany(t => t.Sessions)
               .WithOne(s => s.Trainer)
               .HasForeignKey(s => s.TrainerID)
               .OnDelete(DeleteBehavior.Restrict); // Prevent deleting trainer with sessions

            //Client-Session relationship
            modelBuilder.Entity<Client>()
                .HasMany(c => c.Sessions)
                .WithOne(s => s.Client)
                .HasForeignKey(s => s.ClientID)
                .OnDelete(DeleteBehavior.Restrict); // Prevent deleting client with sessions

        }

    }

    
}