using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Models
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }        

        [Required]
        [Display(Name = "User Id")]
        public string UserId { get; set; }

        [DataType(DataType.DateTime)]        
        public DateTime Date { get; set; }

        [Required]
        public bool IsShipped { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal TotalPrice { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }
        
        public virtual ICollection<CartHistory> CartHistories { get; set; }
    }
}
