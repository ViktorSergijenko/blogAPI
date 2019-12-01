using ProjectStructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectStructure.DTO
{
    public class CommentDTO : BaseDTO
    {
        public Guid PostId { get; set; }
        public Post Post { get; set; }
        public string Text { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public DateTime CommentedAt { get; set; }
        public bool Modified { get; set; }
    }
}
