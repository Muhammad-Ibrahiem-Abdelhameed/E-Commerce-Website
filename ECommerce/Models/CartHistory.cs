using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Models
{
    public class CartHistory
    {
        [Key]       
        public int Id { get; set; } 

        [Required]
        [Display(Name = "Product Id")]
        public string ProductId { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public string OrderId { get; set; }

        [Required]
        public decimal QuantityPrice { get; set; }        

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }

        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; }


    }
}
