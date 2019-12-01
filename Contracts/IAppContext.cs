using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectStructure.Contracts
{
    public interface IAppContext
    {
        /// <summary>
        /// Authorized user id
        /// </summary>
        Guid? CurrentUserId { get; }


        /// <summary>
        /// Указывает, является ли авторизованный пользователь глобальным администратором
        /// </summary>
        bool IsAdmin { get; }

    }
}
