using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace ECommerce.Models
{
    public class ShoppingCart
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        [Required]
        [Display(Name ="Product Id")]
        public string ProductId { get; set; }

        [Required]
        [Display(Name = "User Id")]
        public string UserId { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public decimal QuantityPrice
        {
            get
            {
                return Convert.ToDecimal(Product.Price) * Quantity;
            }
        }

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
        
        [ForeignKey("UserId")]
        public virtual ApplicationUser ApplicationUser  { get; set; }

    }
}
