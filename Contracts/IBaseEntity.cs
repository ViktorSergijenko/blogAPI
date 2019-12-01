using ProjectStructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectStructure.Contracts
{
    public interface IDeletable
    {
        bool IsDeleted { get; set; }

    }

    public interface IBaseEntity
    {
        Guid Id { get; set; }

    }
}
