using Domain.Contracts.Entity;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Infrastructure.Data
{
    public class OnlineCoursesContext : DbContext
    {
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<CourseInstructor> CourseInstructors { get; set; }
        public virtual DbSet<Instructor> Instructors { get; set; }
        public virtual DbSet<Price> Prices { get; set; }

        public OnlineCoursesContext(DbContextOptions<OnlineCoursesContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>(entity =>
            {
                entity.HasIndex(e => e.Title).IsUnique(true);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(500);
                entity.Property(e => e.Description).HasMaxLength(1000);
                entity.Property(e => e.PublishingDate).HasColumnType("DATE");
            });
            GenerateTable<Course>(modelBuilder);

            modelBuilder.Entity<Instructor>(entity =>
            {
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.LastName).HasMaxLength(50);
                entity.Property(e => e.Degree).HasMaxLength(100);
            });
            GenerateTable<Instructor>(modelBuilder);

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.Property(e => e.CourseId).HasColumnType("UNIQUEIDENTIFIER");
                entity.Property(e => e.StudentName).IsRequired().HasMaxLength(60);
                entity.Property(e => e.Message).HasMaxLength(1500);

                entity.HasCheckConstraint($"CK_{nameof(Comment)}_Score", "[Score]>0 AND [Score]<=5");
            });
            GenerateTable<Comment>(modelBuilder);

            modelBuilder.Entity<CourseInstructor>(entity =>
            {
                entity.Property(e => e.CourseId).HasColumnType("UNIQUEIDENTIFIER");
                entity.Property(e => e.InstructorId).HasColumnType("UNIQUEIDENTIFIER");
            });
            GenerateTable<CourseInstructor>(modelBuilder);

            modelBuilder.Entity<Price>(entity =>
            {
                entity.Property(e => e.CourseId).HasColumnType("UNIQUEIDENTIFIER");
                entity.Property(e => e.CurrentPrice).IsRequired().HasPrecision(10, 4);
                entity.Property(e => e.Promotion).HasDefaultValue(0).HasPrecision(10, 4);

                entity.HasCheckConstraint($"CK_{nameof(Price)}_CurrentPrice", "[CurrentPrice]>=0");
                entity.HasCheckConstraint($"CK_{nameof(Price)}_Promotion", "[Promotion]>=0");
            });
            GenerateTable<Price>(modelBuilder);

        }

        private static void GenerateTable<T>(ModelBuilder modelBuilder, string tableName = null) where T : class, IEntity
        {
            modelBuilder.Entity<T>(entity =>
            {
                tableName ??= GetTypeName(entity);
                entity.ToTable(tableName);
                entity.Property(e => e.Id).HasColumnType("UNIQUEIDENTIFIER ROWGUIDCOL").HasDefaultValueSql("NEWSEQUENTIALID()");
                //entity.HasOne<User>().WithOne().HasForeignKey<T>(e => e.CreatedBy).HasConstraintName($"FK_{tableName}_Users_CreatedBy");
                //entity.HasOne<User>().WithOne().HasForeignKey<T>(e => e.UpdatedBy).HasConstraintName($"FK_{tableName}_Users_UpdatedBy");
                //entity.HasIndex(e => e.CreatedBy).IsUnique(false);
                //entity.HasIndex(e => e.UpdatedBy).IsUnique(false);
                entity.Property(e => e.CreatedAt).HasColumnType("DATETIME2(3)").HasDefaultValueSql("GETDATE()");
                entity.Property(e => e.UpdatedAt).HasColumnType("DATETIME2(3)");
                entity.Property(e => e.CreatedBy).HasColumnType("UNIQUEIDENTIFIER");
                entity.Property(e => e.UpdatedBy).HasColumnType("UNIQUEIDENTIFIER");
                entity.Property(e => e.RowVersion).IsRowVersion();
            });
        }

        private static string GetTypeName(object entity)
        {
            return entity.ToString().Split('.').LastOrDefault().Split(']').FirstOrDefault();
        }
    }
}
