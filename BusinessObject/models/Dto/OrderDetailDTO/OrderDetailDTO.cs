using BusinessObject.models.Dto.ProductDto;
using BusinessObject.Object;
using System.ComponentModel.DataAnnotations;

namespace BusinessObject.models.Dto.OrderDetailDTO
{
    public class OrderDetailDTO
    {
		[Required]
		public int OrderId { get; set; }
		//public Order Order { get; set; }

		[Required]
		public int ProductId { get; set; }
		public ProductDTO Product { get; set; }

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
