using Microsoft.AspNetCore.Identity;
using ProjectStructure.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectStructure.Models
{
    public class User : IdentityUser<Guid>, IDeletable
    {
        public User()
        {
            Comments = new List<Comment>();
            Likes = new List<Like>();
            Posts = new List<Post>();
        }
        public string FullName { get; set; }
        public string NickName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Age { get; set; }
        public string RoleName { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public Guid? LanguageId { get; set; }
        public Language Language { get; set; }
        public DateTime RegisteredAt { get; set; }
        public List<Post> Posts { get; set; }
        public List<Like> Likes { get; set; }
        public List<Comment> Comments { get; set; }
        public string AvatarImageBase64 { get; set; }

        #region IDeletable

        public bool IsDeleted { get; set; }

        #endregion IDeletable

        /// <summary>
        /// Navigation property for the roles this user belongs to.
        /// </summary>
        public virtual ICollection<IdentityUserRole<Guid>> Roles { get; } = new List<IdentityUserRole<Guid>>();

        /// <summary>
        /// Navigation property for the claims this user possesses.
        /// </summary>
        public virtual ICollection<IdentityUserClaim<Guid>> Claims { get; } = new List<IdentityUserClaim<Guid>>();

        /// <summary>
        /// Navigation property for this users login accounts.
        /// </summary>
        public virtual ICollection<IdentityUserLogin<Guid>> Logins { get; } = new List<IdentityUserLogin<Guid>>();
    }

    public class Role : IdentityRole<Guid>, IDeletable
    {
        public Role()
        {
        }

        public Role(string roleName)
        {
            Name = roleName;
        }

        #region IDeletable

        public bool IsDeleted { get; set; }

        #endregion IDeletable
    }

    public class UserLogin : IdentityUserLogin<Guid>, IDeletable
    {
        #region IDeletable

        public bool IsDeleted { get; set; }

        #endregion IDeletable
    }

    public class UserClaim : IdentityUserClaim<Guid>, IDeletable
    {
        public User User { get; set; }

        #region IDeletable

        public bool IsDeleted { get; set; }

        #endregion IDeletable

    }
}
