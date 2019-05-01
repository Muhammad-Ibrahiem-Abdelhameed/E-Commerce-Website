using ECommerce.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.ViewModels
{
    public class ProductViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public string Price { get; set; }

        [Required]
        public string Description { get; set; }

        [Display(Name = "Image")]
        public IFormFile Image { get; set; }
    }
}
