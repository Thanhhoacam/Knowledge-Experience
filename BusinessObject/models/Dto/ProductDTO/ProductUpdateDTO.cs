using BusinessObject.Object;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.models.Dto.ProductDto
{
    public class ProductUpdateDTO
    {
        [Required(ErrorMessage = "Product Id is required")]
        public int ProductId { get; set; }
        [Required(ErrorMessage = "Category Id is required")]
        public int CategoryId { get; set; }
        //public Category Category { get; set; }

        [Required(ErrorMessage = "Product name is required"), MaxLength(100)]
        public string ProductName { get; set; }

        [Range(0, double.MaxValue)]
        [Required(ErrorMessage = "Weight is required")]
        public double Weight { get; set; }

        [Range(0, double.MaxValue)]
        [Required(ErrorMessage = "UnitPrice is required")]
        public decimal UnitPrice { get; set; }

        [Range(0, int.MaxValue)]
        [Required(ErrorMessage = "UnitsInStock is required")]
        public int UnitsInStock { get; set; }

        //public ICollection<OrderDetail> OrderDetails { get; set; }

        [Required(ErrorMessage = "ImageUrl is required")]
        public string ImageUrl { get; set; }
    }
}
