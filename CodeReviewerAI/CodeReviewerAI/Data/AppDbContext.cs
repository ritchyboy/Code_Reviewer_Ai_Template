using CodeReviewerAI.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CodeReviewerAI.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<ReviewRecord> ReviewRecords { get; set; }
        public DbSet<FileReview> FileReviews { get; set; }
        public DbSet<AiModelsReviewer> AiModelsReviewers { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> option) : base(option)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ReviewRecord>()
            .HasIndex(r => new { r.RepoName, r.PrNumber })
            .IsUnique();
        }
    }
}
