using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectStructure.ViewModels
{
    public class UserProfileVM
    {
        public string FullName { get; set; }
        public Guid Id { get; set; }
        public int MyProperty { get; set; }
        public string RoleName { get; set; }
        public Guid? CompanyId { get; set; }
    }
}
