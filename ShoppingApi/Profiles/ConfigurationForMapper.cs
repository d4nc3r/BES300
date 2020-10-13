using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingApi.Profiles
{
    public class ConfigurationForMapper
    {
        public readonly string SectionName = "Mapper";
        public decimal markUp { get; set; }
    }

}
