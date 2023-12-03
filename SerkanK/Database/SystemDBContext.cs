using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using SerkanK.Models;

namespace SerkanK.Database
{
    public class SystemDBContext : DbContext
    {

        public DbSet<User> Users { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Card> Cards { get; set; }

        public bool StartUpCheck = false;

        public SystemDBContext(DbContextOptions<SystemDBContext> options) : base (options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Account>().ToTable("Accounts");
            modelBuilder.Entity<Transaction>().ToTable("Transactions");
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

    }
}
