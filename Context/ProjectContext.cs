using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjectStructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectStructure.Context
{
    public class ProjectContext : IdentityDbContext<User, Role, Guid>
    {
        public ProjectContext(DbContextOptions<ProjectContext> options)
           : base(options)
        {

            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder mb)
        {
            #region Post model builder
            mb.Entity<Post>()
                .HasMany(x => x.Comments)
                .WithOne(x => x.Post)
                .HasForeignKey(x => x.PostId)
                ;
            mb.Entity<Post>()
                .HasMany(x => x.Likes)
                .WithOne(x => x.Post)
                .HasForeignKey(x => x.PostId)
                ;
            mb.Entity<Post>()
                .HasOne(x => x.User)
                .WithMany(x => x.Posts)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict)
                ;
            #endregion Post model builder

            #region User model builder
            mb.Entity<User>()
                .HasMany(x => x.Likes)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId)
                ;
            mb.Entity<User>()
                .HasMany(x => x.Posts)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId)
                ;
            mb.Entity<User>()
                .HasMany(x => x.Comments)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId)
                ;
            mb.Entity<User>()
                .HasOne(x => x.Language)
                .WithMany(x => x.Users)
                .HasForeignKey(x => x.LanguageId)
                ;
            #endregion User model builder

            #region Like model builder
            mb.Entity<Like>()
                .HasOne(x => x.User)
                .WithMany(x => x.Likes)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict)
                ;
            mb.Entity<Like>()
                .HasOne(x => x.Post)
                .WithMany(x => x.Likes)
                .HasForeignKey(x => x.PostId)
                .OnDelete(DeleteBehavior.Restrict)
                ;
            #endregion Like model builder

            #region Comment model builder
            mb.Entity<Comment>()
                .HasOne(x => x.User)
                .WithMany(x => x.Comments)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict)
                ;
            mb.Entity<Comment>()
                .HasOne(x => x.Post)
                .WithMany(x => x.Comments)
                .HasForeignKey(x => x.PostId)
                .OnDelete(DeleteBehavior.Restrict)
                ;
            #endregion Comment model builder

            #region Language
            mb.Entity<Language>()
                .HasMany(x => x.Users)
                .WithOne(x => x.Language)
                .HasForeignKey(x => x.LanguageId)
                .OnDelete(DeleteBehavior.Restrict)
                ;
            #endregion Language

            base.OnModelCreating(mb);

            #region ASP.NET Identity Table Names

            mb.Entity<IdentityUserRole<Guid>>().ToTable("_UsersRoles");
            mb.Entity<IdentityUserClaim<Guid>>().ToTable("_UsersClaims");
            mb.Entity<IdentityUserLogin<Guid>>().ToTable("_UsersLogins");
            mb.Entity<IdentityRoleClaim<Guid>>().ToTable("_RoleClaims");
            mb.Entity<IdentityUserToken<Guid>>().ToTable("_UserTokens");
            #endregion ASP.NET Identity Table Names
        }

        #region DbSets
        public DbSet<Language> Languages { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Like> Likes { get; set; }
        #endregion DbSets
    }
}
