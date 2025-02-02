using BusinessObject.Object;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.models.Dto.MemberDTO
{
    public class MemberCreateDTO
    {
        //[Key]
        //public int MemberId { get; set; }

        [Required, EmailAddress(ErrorMessage = "Enter Email")]
        public string Email { get; set; }

        public string CompanyName { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        [Required, MinLength(6, ErrorMessage = "Enter at least 6 char")]
        public string Password { get; set; }
        public string Role { get; set; }


        //public ICollection<Order> Orders { get; set; }
    }
}
