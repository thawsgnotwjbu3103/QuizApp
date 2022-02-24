using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace QuizApp.Models
{
    public partial class testContext : DbContext
    {
        public testContext()
        {
        }

        public testContext(DbContextOptions<testContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Notification> Notifications { get; set; }
        public virtual DbSet<Point> Points { get; set; }
        public virtual DbSet<Question> Questions { get; set; }
        public virtual DbSet<QuestionChoice> QuestionChoices { get; set; }
        public virtual DbSet<QuestionsText> QuestionsTexts { get; set; }
        public virtual DbSet<TblQuiz> TblQuizzes { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserAnswer> UserAnswers { get; set; }
        public virtual DbSet<UserAnswerText> UserAnswerTexts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=THANG-PC;Database=test;Trusted_Connection=True;Integrated Security=True;MultipleActiveResultSets=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Notification>(entity =>
            {
                entity.HasKey(e => e.NotifyId);

                entity.Property(e => e.DateCreated)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.DateUpdated)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.NotifyContent).IsRequired();

                entity.Property(e => e.Title).IsRequired();
            });

            modelBuilder.Entity<Point>(entity =>
            {
                entity.Property(e => e.PointId).ValueGeneratedOnAdd();

                entity.HasOne(d => d.PointNavigation)
                    .WithOne(p => p.Point)
                    .HasForeignKey<Point>(d => d.PointId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Points_Users");
            });

            modelBuilder.Entity<Question>(entity =>
            {
                entity.Property(e => e.QuestionTitle).IsRequired();

                entity.HasOne(d => d.Quiz)
                    .WithMany(p => p.Questions)
                    .HasForeignKey(d => d.QuizId)
                    .HasConstraintName("FK_Questions_Quiz");
            });

            modelBuilder.Entity<QuestionChoice>(entity =>
            {
                entity.HasKey(e => e.ChoiceId);

                entity.ToTable("Question_choices");

                entity.Property(e => e.Choice).IsRequired();

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.QuestionChoices)
                    .HasForeignKey(d => d.QuestionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Question_choices_Questions");
            });

            modelBuilder.Entity<QuestionsText>(entity =>
            {
                entity.HasKey(e => e.QuestionTextId);

                entity.ToTable("QuestionsText");

                entity.Property(e => e.QuestionTextTitle)
                    .HasMaxLength(10)
                    .IsFixedLength(true);

                entity.HasOne(d => d.Quiz)
                    .WithMany(p => p.QuestionsTexts)
                    .HasForeignKey(d => d.QuizId)
                    .HasConstraintName("FK_QuestionsText_TblQuiz");
            });

            modelBuilder.Entity<TblQuiz>(entity =>
            {
                entity.HasKey(e => e.QuizId)
                    .HasName("PK_Quiz");

                entity.ToTable("TblQuiz");

                entity.Property(e => e.DateCreated)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.QuizName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Time)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Birthday)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.DateCreated)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FullName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Gender)
                    .IsRequired()
                    .HasMaxLength(3);

                entity.Property(e => e.IdNum)
                    .IsRequired()
                    .HasMaxLength(12)
                    .IsUnicode(false);

                entity.Property(e => e.PhoneNum)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<UserAnswer>(entity =>
            {
                entity.HasKey(e => e.UserAnswersId);

                entity.ToTable("User_Answers");

                entity.HasOne(d => d.Choice)
                    .WithMany(p => p.UserAnswers)
                    .HasForeignKey(d => d.ChoiceId)
                    .HasConstraintName("FK_User_Answers_Question_choices");

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.UserAnswers)
                    .HasForeignKey(d => d.QuestionId)
                    .HasConstraintName("FK_User_Answers_Questions");

                entity.HasOne(d => d.Quiz)
                    .WithMany(p => p.UserAnswers)
                    .HasForeignKey(d => d.QuizId)
                    .HasConstraintName("FK_User_Answers_Quiz");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserAnswers)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_User_Answers_Users");
            });

            modelBuilder.Entity<UserAnswerText>(entity =>
            {
                entity.HasKey(e => e.UaTextId);

                entity.Property(e => e.UaTextId).HasColumnName("uaTextId");

                entity.Property(e => e.Matches)
                    .HasMaxLength(10)
                    .IsFixedLength(true);

                entity.HasOne(d => d.QuestionText)
                    .WithMany(p => p.UserAnswerTexts)
                    .HasForeignKey(d => d.QuestionTextId)
                    .HasConstraintName("FK_UserAnswerTexts_QuestionsText");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserAnswerTexts)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_UserAnswerTexts_Users");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
