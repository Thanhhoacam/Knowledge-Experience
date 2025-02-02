using BusinessObject.models.Dto.ProductDto;
using BusinessObject.Object;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.models.Dto.CategoryDTO
{
    public class CategoryDTO
    {
        public int CategoryId { get; set; }

        [Required, MaxLength(100)]
        public string CategoryName { get; set; }

        //public ICollection<ProductDTO> Products { get; set; } // tranh vong lap serialize
    }
}
