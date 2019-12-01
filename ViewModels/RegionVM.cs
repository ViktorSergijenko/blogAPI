using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectStructure.ViewModels
{
    public class RegionVM : BaseVM
    {
        public string RegionName { get; set; }
        [Range(-12, 12)]
        public int DifferenceInHoursTime { get; set; }
    }
}
