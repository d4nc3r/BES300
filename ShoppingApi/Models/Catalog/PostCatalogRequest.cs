using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingApi.Models.Catalog
{
    public class PostCatalogRequest
    {
        [Required]
        public string Description { get; set; }
        [Required]
        public decimal Cost { get; set; }
    }
}
