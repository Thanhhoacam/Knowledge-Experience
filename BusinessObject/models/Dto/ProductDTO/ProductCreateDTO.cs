using BusinessObject.Object;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.models.Dto.ProductDto
{
    public class ProductCreateDTO
    {
        //public int ProductId { get; set; }
        [Required(ErrorMessage = "Category is required")]
        public int CategoryId { get; set; }

        //public Category Category { get; set; }

        [Required(ErrorMessage = "Product name is required"), MaxLength(100)]
        public string ProductName { get; set; }

        public double Weight { get; set; }

        public decimal UnitPrice { get; set; }

        public int UnitsInStock { get; set; }
        public string ImageUrl { get; set; }

        //public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
