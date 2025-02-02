using BusinessObject.Object;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.models.Dto.MemberDTO
{
    public class MemberUpdateDTO
    {
        [Required]
        public int MemberId { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, MaxLength(100)]
        public string CompanyName { get; set; }

        [Required, MaxLength(50)]
        public string City { get; set; }

        [Required, MaxLength(50)]
        public string Country { get; set; }

        [Required, MinLength(6)]
        public string Password { get; set; }
        public string Role { get; set; }


        //public ICollection<Order> Orders { get; set; }
    }
}
