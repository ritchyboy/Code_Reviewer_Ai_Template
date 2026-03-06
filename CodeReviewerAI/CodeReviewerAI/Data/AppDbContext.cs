using CodeReviewerAI.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeReviewerAI.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<ReviewRecord> ReviewRecords { get; set; }
        public DbSet<FileReview> FileReviews { get; set; }
        public DbSet<AiModelsReviewer> AiModelsReviewers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            var connectionString = Environment.GetEnvironmentVariable("SQL_CONNECTION_STRING")
            ?? "Server=localhost;Database=CodeReviewerDB;Trusted_Connection=True;TrustServerCertificate=True;";

            options.UseSqlServer(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ReviewRecord>()
            .HasIndex(r => new { r.RepoName, r.PrNumber })
            .IsUnique();
        }
    }
}
