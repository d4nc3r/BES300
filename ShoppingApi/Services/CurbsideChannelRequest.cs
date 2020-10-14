using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingApi.Services
{
    public class CurbsideChannelRequest
    {
        public int OrderId { get; set; }
        public string ClientId { get; set; }
    }
}
