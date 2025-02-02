using BusinessObject.Object;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.models.Dto.OrderDetailDTO
{
    public class OrderDetailCreateDTO
    {
        [Required]
        public int OrderId { get; set; }
        //public Order Order { get; set; }

        [Required]
        public int ProductId { get; set; }
        //public Product Product { get; set; }

        [Range(0, double.MaxValue)]
        [Required]
        public decimal UnitPrice { get; set; }

        [Range(1, int.MaxValue)]
        [Required]
        public int Quantity { get; set; }

        [Range(0, 1)]
        [Required]
        public double Discount { get; set; }
    }
}
