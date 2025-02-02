using System.ComponentModel.DataAnnotations;

namespace BusinessObject.Object
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        [Required(ErrorMessage = "Product name is required"), MaxLength(100)]
        public string ProductName { get; set; }

        [Range(0.1, double.MaxValue, ErrorMessage = "Weight at least 0.1")]
        public double Weight { get; set; }

        [Range(0, double.MaxValue)]
        public decimal UnitPrice { get; set; }

        [Range(0, int.MaxValue)]
        public int UnitsInStock { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; }
        public string ImageUrl { get; set; }
    }
}
