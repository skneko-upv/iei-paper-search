using IEIPaperSearch.Models;
using Microsoft.EntityFrameworkCore;

namespace IEIPaperSearch.Persistence
{
    public class PaperSearchContext : DbContext
    {
#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        public PaperSearchContext(DbContextOptions<PaperSearchContext> options) : base(options)
        { }
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.

        public DbSet<Article> Articles { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<InProceedings> InProceedings { get; set; }
        public DbSet<Journal> Journals { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<Issue> Issues { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(options =>
            {
                options.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
            });
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Submission>()
                .HasMany(s => s.Authors).WithMany(p => p.AuthorOf);
        }
    }
}
