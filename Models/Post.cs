using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectStructure.Models
{
    public class Post : BaseEntity
    {
        public Post()
        {
            Comments = new List<Comment>();
            Likes = new List<Like>();
        }
        public string Title { get; set; }
        public string AuthorFullName { get; set; }
        public string Text { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public DateTime PostedAt { get; set; }
        public List<Comment> Comments { get; set; }
        public List<Like> Likes { get; set; }
        public string ImageBase64 { get; set; }
    }
}
