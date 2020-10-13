using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingApi.Domain
{
    public enum CurbsideOrderStatus { Pending, Approved, Denied, Fulfilled }
    public class CurbsideOrder
    {
        public int Id { get; set; }
        public string For { get; set; }
        public string Items { get; set; } // "1,2,3"
        public CurbsideOrderStatus Status { get; set; }
    }
}
