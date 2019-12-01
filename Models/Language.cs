using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectStructure.Models
{
    /// <summary>
    /// Languages
    /// </summary>
    public class Language : BaseEntity
    {
        /// <summary>
        /// Language  title
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Language ISO code (2 letter)
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Show is this language default 
        /// </summary>
        public bool IsDefault { get; set; }
        public List<User> Users { get; set; }

        #region Переопределение ToString()
        public override string ToString() { return Code; }
        #endregion
    }

}
