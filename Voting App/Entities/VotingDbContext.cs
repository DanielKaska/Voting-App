﻿using Microsoft.EntityFrameworkCore;

namespace Voting_App.Entities
{
    public class VotingDbContext : DbContext
    {
        private string connectionString = "Server=localhost;Database=VotingAppDB;Integrated Security=True;TrustServerCertificate=True;";
        public DbSet<User> Users { get; set; }
        public DbSet<Vote> Votes { get; set; }
        public DbSet<Answer> Answers { get; set; }

        protected override void OnModelCreating(ModelBuilder mb)
        {
            mb.Entity<User>().Property(u => u.Id).IsRequired();
            mb.Entity<User>().Property(u => u.Email).IsRequired();
        }

        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString);
        }

    }
}
