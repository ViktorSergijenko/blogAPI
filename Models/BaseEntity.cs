using ProjectStructure.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectStructure.Models
{
    public class BaseEntity : IBaseEntity, IDeletable
    {
        #region IBaseEntity

        [Key]
        public Guid Id { get; set; }
        #endregion IBaseEntity

        #region IDeletable

        public bool IsDeleted { get; set; }

        #endregion IDeletable
    }
}
