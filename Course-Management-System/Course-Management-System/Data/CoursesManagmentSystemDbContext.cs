using Course_Management_System.Models.Domain;
using CourseManagementSystem.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Course_Management_System.Data
{
    public class CoursesManagmentSystemDbContext : DbContext
    {
        public CoursesManagmentSystemDbContext(DbContextOptions<CoursesManagmentSystemDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Enrollment -> ApplicationUser relationship
            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Student)
                .WithMany()
                .HasForeignKey(e => e.StudentId)
                .OnDelete(DeleteBehavior.Restrict); // Deleting a user deletes enrollments

            // Configure Enrollment -> Course relationship
            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Course)
                .WithMany(c => c.Enrollments)
                .HasForeignKey(e => e.CourseId)
                .OnDelete(DeleteBehavior.Cascade); // Deleting a course deletes enrollments


            // Configure QuizAnswer -> QuizQuestion relationship
            modelBuilder.Entity<QuizAnswer>()
                .HasOne(qa => qa.Question)
                .WithMany()
                .HasForeignKey(qa => qa.QuizQuestionId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete for QuizQuestion

            // Configure QuizQuestion -> Quiz relationship
            modelBuilder.Entity<QuizQuestion>()
                .HasOne(qq => qq.Quiz)
                .WithMany(q => q.Questions)
                .HasForeignKey(qq => qq.QuizId)
                .OnDelete(DeleteBehavior.Cascade); // Allow cascade delete for Quiz

            // Configure CompletedLesson -> ApplicationUser relationship
            modelBuilder.Entity<CompletedLesson>()
                .HasOne(cl => cl.User)
                .WithMany()
                .HasForeignKey(cl => cl.StudentId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete for Student

            // Configure CompletedLesson -> Lesson relationship
            modelBuilder.Entity<CompletedLesson>()
                .HasOne(cl => cl.Lesson)
                .WithMany()
                .HasForeignKey(cl => cl.LessonId)
                .OnDelete(DeleteBehavior.Cascade); // Allow cascade delete for Lesson
        }

        public DbSet<Course> Courses { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<Video> Videos { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<QuizQuestion> QuizQuestions { get; set; }
        public DbSet<QuizAttempt> QuizAttempts { get; set; }
        public DbSet<QuizAnswer> QuizAnswers { get; set; }
        public DbSet<CompletedLesson> CompletedLessons { get; set; }
    }
}
