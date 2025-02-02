using AutoMapper;
using BusinessObject.models.Dto.CategoryDTO;
using BusinessObject.models.Dto.MemberDTO;
using BusinessObject.models.Dto.OrderDetailDTO;
using BusinessObject.models.Dto.OrderDTO;
using BusinessObject.models.Dto.ProductDto;
using BusinessObject.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.models
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            //v1
            //CreateMap<Source, Destination>();
            //CreateMap<Destination, Source>();
            CreateMap<Product, ProductDTO>();
            CreateMap<ProductDTO, Product>();

            //v2
            //CreateMap<Source, Destination>().ReverseMap();
            CreateMap<Product, ProductCreateDTO>().ReverseMap();
            CreateMap<Product, ProductUpdateDTO>().ReverseMap();

            CreateMap<ProductDTO, ProductCreateDTO>().ReverseMap();
            CreateMap<ProductDTO, ProductUpdateDTO>().ReverseMap();

            //create map for category reverse map
            CreateMap<Category, CategoryDTO>().ReverseMap();
            CreateMap<Category, CategoryCreateDTO>().ReverseMap();
            CreateMap<Category, CategoryUpdateDTO>().ReverseMap();

            CreateMap<CategoryDTO, CategoryCreateDTO>().ReverseMap();
            CreateMap<CategoryDTO, CategoryUpdateDTO>().ReverseMap();

            //create map for orderdetail reverse map
            CreateMap<OrderDetail, OrderDetailDTO>().ReverseMap();
            CreateMap<OrderDetail, OrderDetailCreateDTO>().ReverseMap();
            CreateMap<OrderDetail, OrderDetailUpdateDTO>().ReverseMap();

            CreateMap<OrderDetailDTO, OrderDetailCreateDTO>().ReverseMap();
            CreateMap<OrderDetailDTO, OrderDetailUpdateDTO>().ReverseMap();

            //create map for order reverse map
            CreateMap<Order, OrderDTO>().ReverseMap();
            CreateMap<Order, OrderCreateDTO>().ReverseMap();
            CreateMap<Order, OrderUpdateDTO>().ReverseMap();

            CreateMap<OrderDTO, OrderCreateDTO>().ReverseMap();
            CreateMap<OrderDTO, OrderUpdateDTO>().ReverseMap();

            //create map for member reverse map
            CreateMap<Member, MemberDTO>().ReverseMap();

            CreateMap<Member, MemberCreateDTO>().ReverseMap();
            CreateMap<Member, MemberUpdateDTO>().ReverseMap();

            CreateMap<MemberDTO, MemberCreateDTO>().ReverseMap();
            CreateMap<MemberDTO, MemberUpdateDTO>().ReverseMap();
            
           
        }
    }
}
