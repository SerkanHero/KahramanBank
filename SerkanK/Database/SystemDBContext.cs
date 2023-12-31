using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using SerkanK.Models;

namespace SerkanK.Database
{
    public class SystemDBContext : DbContext
    {

        public DbSet<Admin> Admins { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Card> Cards { get; set; }

        public bool StartUpCheck = false;

        public SystemDBContext(DbContextOptions<SystemDBContext> options) : base (options)
        {
            UpdateDatabaseSchema();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Admin>().ToTable("Admins");
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Account>().ToTable("Accounts");
            modelBuilder.Entity<Transaction>().ToTable("Transactions");
            modelBuilder.Entity<Card>().ToTable("Cards");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=bank.db");
        }

        public void StartUp()
        {
            if(!StartUpCheck)
                this.Database.EnsureCreated();
            StartUpCheck = true;
        }

        public void UpdateDatabaseSchema()
        {
            try
            {
                var pendingMigrations = this.Database.GetPendingMigrations();
                if (pendingMigrations.Any())
                {
                    this.Database.Migrate();
                    Console.WriteLine("Database schema updated successfully.");
                }
                else
                {
                    Console.WriteLine("Database schema is up to date.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while updating the database schema: {ex.Message}");
            }
        }

    }
}
