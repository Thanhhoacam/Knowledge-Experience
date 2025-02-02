using System.ComponentModel.DataAnnotations;

namespace BusinessObject.models.Dto.ProductDto
{
	public class ProductDTO
    {
        public int ProductId { get; set; }

        public int CategoryId { get; set; }
        public CategoryDTO.CategoryDTO Category { get; set; }
        [Required, MaxLength(100)]
        public string ProductName { get; set; }

        [Range(0.1, double.MaxValue, ErrorMessage = "Weight at least 0.1")]
        public double Weight { get; set; }

        [Range(0, double.MaxValue)]
        public decimal UnitPrice { get; set; }

        [Range(0, int.MaxValue)]
        public int UnitsInStock { get; set; }

        //public ICollection<OrderDetailDTO.OrderDetailDTO> OrderDetails { get; set; } // tránh vòng lặp serialize json

        public string ImageUrl { get; set; }

    }
}
