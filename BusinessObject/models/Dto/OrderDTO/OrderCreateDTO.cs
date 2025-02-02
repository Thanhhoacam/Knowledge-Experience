using BusinessObject.Object;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.models.Dto.OrderDTO
{
    public class OrderCreateDTO
    {
        //[Key]
        //[Required]
        //public int OrderId { get; set; }

        [Required]
        public int MemberId { get; set; }
        //public Member Member { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        public DateTime RequiredDate { get; set; }

        public DateTime ShippedDate { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Freight { get; set; }

        //public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
