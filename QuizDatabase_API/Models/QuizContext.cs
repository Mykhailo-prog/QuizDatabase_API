using Microsoft.EntityFrameworkCore;

namespace QuizDatabase_API.Models
{

    public class QuizContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public QuizContext(DbContextOptions<QuizContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().Property(x => x.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Question>().Property(x => x.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Answer>().Property(x => x.Id).ValueGeneratedOnAdd();
        }
    }
}