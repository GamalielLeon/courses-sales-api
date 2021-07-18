using Domain.Contracts.Entity;
using Domain.DTOs.Pagination;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

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

        //Entities not mapped in the database.
        public virtual DbSet<CoursesPaged> CoursesPageds { get; set; }
        public virtual DbSet<CommentsPaged> CommentsPaged { get; set; }
        public virtual DbSet<InstructorsPaged> InstructorsPaged { get; set; }
        public virtual DbSet<PricesPaged> PricesPaged { get; set; }
        public virtual DbSet<RolesPaged> RolesPaged { get; set; }
        public virtual DbSet<UsersPaged> UsersPaged { get; set; }


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
            ConfigureTable<User>(builder);

            builder.Entity<Role>(static entity =>
            {
                entity.HasIndex(static e => e.Code).IsUnique(true);
                entity.Property(static e => e.Code).IsRequired().HasMaxLength(10);
                entity.HasIndex(static e => e.Name).IsUnique(true);
                entity.Property(static e => e.Name).IsRequired().HasMaxLength(30);
            });
            ConfigureTable<Role>(builder);

            builder.Entity<Course>(static entity =>
            {
                entity.HasIndex(static e => e.Title).IsUnique(true);
                entity.Property(static e => e.Title).IsRequired().HasMaxLength(500);
                entity.Property(static e => e.Description).HasMaxLength(1000);
                entity.Property(static e => e.PublishingDate).HasColumnType("DATE");
            });
            ConfigureTable<Course>(builder);

            builder.Entity<Instructor>(static entity =>
            {
                entity.Property(static e => e.FirstName).IsRequired().HasMaxLength(50);
                entity.Property(static e => e.LastName).HasMaxLength(50);
                entity.Property(static e => e.Degree).IsRequired().HasMaxLength(100);
            });
            ConfigureTable<Instructor>(builder);

            builder.Entity<Comment>(static entity =>
            {
                entity.Property(static e => e.CourseId).HasColumnType("UNIQUEIDENTIFIER");
                entity.Property(static e => e.StudentName).IsRequired().HasMaxLength(60);
                entity.Property(static e => e.Message).HasMaxLength(1500);

                entity.HasCheckConstraint($"CK_{nameof(Comment)}_Score", "[Score]>0 AND [Score]<=5");
            });
            ConfigureTable<Comment>(builder);

            builder.Entity<CourseInstructor>(static entity =>
            {
                entity.Property(static e => e.CourseId).HasColumnType("UNIQUEIDENTIFIER");
                entity.Property(static e => e.InstructorId).HasColumnType("UNIQUEIDENTIFIER");
            });
            ConfigureTable<CourseInstructor>(builder);

            builder.Entity<Price>(static entity =>
            {
                entity.Property(static e => e.CourseId).HasColumnType("UNIQUEIDENTIFIER");
                entity.Property(static e => e.CurrentPrice).IsRequired().HasPrecision(10, 4);
                entity.Property(static e => e.Promotion).HasDefaultValue(0).HasPrecision(10, 4);

                entity.HasCheckConstraint($"CK_{nameof(Price)}_CurrentPrice", "[CurrentPrice]>=0");
                entity.HasCheckConstraint($"CK_{nameof(Price)}_Promotion", "[Promotion]>=0");
            });
            ConfigureTable<Price>(builder);

            ExcludeTable<CoursesPaged>(builder);
            ExcludeTable<CommentsPaged>(builder);
            ExcludeTable<InstructorsPaged>(builder);
            ExcludeTable<PricesPaged>(builder);
            ExcludeTable<UsersPaged>(builder);
            ExcludeTable<RolesPaged>(builder);
        }

        private static void ConfigureTable<T>(ModelBuilder builder, string tableName = null) where T : class, IEntity, IRowVersion
        {
            builder.Entity<T>(entity =>
            {
                tableName ??= GetTypeName<T>();
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

        public static void ConfigureView<T>(ModelBuilder builder, string viewName = null) where T : class
        {
            builder.Entity<T>(viewEntity =>
            {
                viewEntity.HasNoKey();
                viewEntity.ToView(viewName ?? GetTypeName<T>());
            });
        }

        private static void ExcludeTable<T>(ModelBuilder builder, string tableName = null) where T : class
        {
            builder.Entity<T>(entity =>
            {
                entity.HasNoKey();
                entity.ToView(null);
                entity.ToTable(tableName ?? GetTypeName<T>(), static t => t.ExcludeFromMigrations());
            });
        }
        private static string GetTypeName<T>() where T : class => typeof(T).Name;
    }
}
