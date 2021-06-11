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
            modelBuilder.Entity<Course>(static entity =>
            {
                entity.HasIndex(static e => e.Title).IsUnique(true);
                entity.Property(static e => e.Title).IsRequired().HasMaxLength(500);
                entity.Property(static e => e.Description).HasMaxLength(1000);
                entity.Property(static e => e.PublishingDate).HasColumnType("DATE");
            });
            GenerateTable<Course>(modelBuilder);

            modelBuilder.Entity<Instructor>(static entity =>
            {
                entity.Property(static e => e.FirstName).IsRequired().HasMaxLength(50);
                entity.Property(static e => e.LastName).HasMaxLength(50);
                entity.Property(static e => e.Degree).HasMaxLength(100);
            });
            GenerateTable<Instructor>(modelBuilder);

            modelBuilder.Entity<Comment>(static entity =>
            {
                entity.Property(static e => e.CourseId).HasColumnType("UNIQUEIDENTIFIER");
                entity.Property(static e => e.StudentName).IsRequired().HasMaxLength(60);
                entity.Property(static e => e.Message).HasMaxLength(1500);

                entity.HasCheckConstraint($"CK_{nameof(Comment)}_Score", "[Score]>0 AND [Score]<=5");
            });
            GenerateTable<Comment>(modelBuilder);

            modelBuilder.Entity<CourseInstructor>(static entity =>
            {
                entity.Property(static e => e.CourseId).HasColumnType("UNIQUEIDENTIFIER");
                entity.Property(static e => e.InstructorId).HasColumnType("UNIQUEIDENTIFIER");
            });
            GenerateTable<CourseInstructor>(modelBuilder);

            modelBuilder.Entity<Price>(static entity =>
            {
                entity.Property(static e => e.CourseId).HasColumnType("UNIQUEIDENTIFIER");
                entity.Property(static e => e.CurrentPrice).IsRequired().HasPrecision(10, 4);
                entity.Property(static e => e.Promotion).HasDefaultValue(0).HasPrecision(10, 4);

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
                entity.Property(static e => e.Id).HasColumnType("UNIQUEIDENTIFIER ROWGUIDCOL").HasDefaultValueSql("NEWSEQUENTIALID()");
                //entity.HasOne<User>().WithOne().HasForeignKey<T>(e => e.CreatedBy).HasConstraintName($"FK_{tableName}_Users_CreatedBy");
                //entity.HasOne<User>().WithOne().HasForeignKey<T>(e => e.UpdatedBy).HasConstraintName($"FK_{tableName}_Users_UpdatedBy");
                //entity.HasIndex(e => e.CreatedBy).IsUnique(false);
                //entity.HasIndex(e => e.UpdatedBy).IsUnique(false);
                entity.Property(static e => e.CreatedAt).HasColumnType("DATETIME2(3)").HasDefaultValueSql("GETDATE()");
                entity.Property(static e => e.UpdatedAt).HasColumnType("DATETIME2(3)");
                entity.Property(static e => e.CreatedBy).HasColumnType("UNIQUEIDENTIFIER");
                entity.Property(static e => e.UpdatedBy).HasColumnType("UNIQUEIDENTIFIER");
                entity.Property(static e => e.RowVersion).IsRowVersion();
            });
        }

        private static string GetTypeName(object entity) => entity.ToString().Split('.').LastOrDefault().Split(']').FirstOrDefault();
    }
}
