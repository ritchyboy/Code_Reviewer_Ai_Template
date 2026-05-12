using CodeReviewerAI.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CodeReviewerAI.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<ReviewRecord> ReviewRecords { get; set; }
        public DbSet<FileReview> FileReviews { get; set; }
        public DbSet<AiModelsReviewer> AiModelsReviewers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            var connectionString = Environment.GetEnvironmentVariable("SQL_CONNECTION_STRING");
            if (string.IsNullOrEmpty(connectionString))
            {
                connectionString = Environment.GetEnvironmentVariable("SQL_CONNECTION_STRING"
                ,EnvironmentVariableTarget.User);
            }

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
