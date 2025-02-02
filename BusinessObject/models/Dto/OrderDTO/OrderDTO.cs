using BusinessObject.Object;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.models.Dto.OrderDTO
{
    public class OrderDTO
    {
        
        public int OrderId { get; set; }

        public int MemberId { get; set; }
        public MemberDTO.MemberDTO Member { get; set; }

        [Required]
		[DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
		public DateTime OrderDate { get; set; }

        [Required]
		[DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
		public DateTime RequiredDate { get; set; }
		[DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
		public DateTime ShippedDate { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Freight { get; set; }

        public ICollection<OrderDetailDTO.OrderDetailDTO> OrderDetails { get; set; }
    }
}
