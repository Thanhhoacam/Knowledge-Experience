using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Object
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        public int MemberId { get; set; }
        public Member Member { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        public DateTime RequiredDate { get; set; }

        public DateTime ShippedDate { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Freight { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
