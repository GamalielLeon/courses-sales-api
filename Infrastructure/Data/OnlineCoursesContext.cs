using Domain.Contracts.Entity;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Infrastructure.Data
{
    public class OnlineCoursesContext : IdentityDbContext<User, Role, Guid>
    {
        public OnlineCoursesContext(DbContextOptions<OnlineCoursesContext> options) : base(options)
        {
        }

        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<CourseInstructor> CourseInstructors { get; set; }
        public virtual DbSet<Instructor> Instructors { get; set; }
        public virtual DbSet<Price> Prices { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<IdentityUserRole<Guid>>(static entity =>
            {
                entity.Property(e => e.RoleId).HasColumnType("UNIQUEIDENTIFIER");
                entity.Property(e => e.UserId).HasColumnType("UNIQUEIDENTIFIER");
            });

            builder.Entity<IdentityUserClaim<Guid>>(static entity =>
            {
                entity.Property(e => e.Id).HasColumnType("UNIQUEIDENTIFIER ROWGUIDCOL").HasDefaultValueSql("NEWSEQUENTIALID()");
                entity.Property(e => e.UserId).HasColumnType("UNIQUEIDENTIFIER");
            });

            builder.Entity<IdentityUserLogin<Guid>>(static entity =>
            {
                entity.Property(e => e.UserId).HasColumnType("UNIQUEIDENTIFIER");
            });

            builder.Entity<IdentityRoleClaim<Guid>>(static entity =>
            {
                entity.Property(e => e.Id).HasColumnType("UNIQUEIDENTIFIER ROWGUIDCOL").HasDefaultValueSql("NEWSEQUENTIALID()");
                entity.Property(e => e.RoleId).HasColumnType("UNIQUEIDENTIFIER");
            });

            builder.Entity<IdentityUserToken<Guid>>(static entity =>
            {
                entity.Property(e => e.UserId).HasColumnType("UNIQUEIDENTIFIER");
            });

            builder.Entity<User>(static entity =>
            {
                entity.HasIndex(e => e.Email).IsUnique(true);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(50);
                entity.HasIndex(e => e.UserName).IsUnique(true);
                entity.Property(e => e.UserName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.PasswordHash).HasMaxLength(128);
                entity.Property(e => e.FirsName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.PhoneNumber).HasMaxLength(10);
            });
            GenerateTable<User>(builder);

            builder.Entity<Role>(static entity =>
            {
                entity.HasIndex(static e => e.Code).IsUnique(true);
                entity.Property(static e => e.Code).IsRequired().HasMaxLength(10);
                entity.HasIndex(static e => e.Name).IsUnique(true);
                entity.Property(static e => e.Name).IsRequired().HasMaxLength(30);
            });
            GenerateTable<Role>(builder);

            builder.Entity<Course>(static entity =>
            {
                entity.HasIndex(static e => e.Title).IsUnique(true);
                entity.Property(static e => e.Title).IsRequired().HasMaxLength(500);
                entity.Property(static e => e.Description).HasMaxLength(1000);
                entity.Property(static e => e.PublishingDate).HasColumnType("DATE");
            });
            GenerateTable<Course>(builder);

            builder.Entity<Instructor>(static entity =>
            {
                entity.Property(static e => e.FirstName).IsRequired().HasMaxLength(50);
                entity.Property(static e => e.LastName).HasMaxLength(50);
                entity.Property(static e => e.Degree).IsRequired().HasMaxLength(100);
            });
            GenerateTable<Instructor>(builder);

            builder.Entity<Comment>(static entity =>
            {
                entity.Property(static e => e.CourseId).HasColumnType("UNIQUEIDENTIFIER");
                entity.Property(static e => e.StudentName).IsRequired().HasMaxLength(60);
                entity.Property(static e => e.Message).HasMaxLength(1500);

                entity.HasCheckConstraint($"CK_{nameof(Comment)}_Score", "[Score]>0 AND [Score]<=5");
            });
            GenerateTable<Comment>(builder);

            builder.Entity<CourseInstructor>(static entity =>
            {
                entity.Property(static e => e.CourseId).HasColumnType("UNIQUEIDENTIFIER");
                entity.Property(static e => e.InstructorId).HasColumnType("UNIQUEIDENTIFIER");
            });
            GenerateTable<CourseInstructor>(builder);

            builder.Entity<Price>(static entity =>
            {
                entity.Property(static e => e.CourseId).HasColumnType("UNIQUEIDENTIFIER");
                entity.Property(static e => e.CurrentPrice).IsRequired().HasPrecision(10, 4);
                entity.Property(static e => e.Promotion).HasDefaultValue(0).HasPrecision(10, 4);

                entity.HasCheckConstraint($"CK_{nameof(Price)}_CurrentPrice", "[CurrentPrice]>=0");
                entity.HasCheckConstraint($"CK_{nameof(Price)}_Promotion", "[Promotion]>=0");
            });
            GenerateTable<Price>(builder);
        }

        private static void GenerateTable<T>(ModelBuilder builder, string tableName = null) where T : class, IEntity
        {
            builder.Entity<T>(entity =>
            {
                tableName ??= GetTypeName(entity);
                entity.ToTable(tableName);
                entity.Property(static e => e.Id).HasColumnType("UNIQUEIDENTIFIER ROWGUIDCOL").HasDefaultValueSql("NEWSEQUENTIALID()");
                //entity.HasOne<User>().WithOne().HasForeignKey<T>(e => e.CreatedBy).HasConstraintName($"FK_{tableName}_Users_CreatedBy");
                //entity.HasOne<User>().WithOne().HasForeignKey<T>(e => e.UpdatedBy).HasConstraintName($"FK_{tableName}_Users_UpdatedBy");
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
