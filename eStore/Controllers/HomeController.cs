using AutoMapper;
using BusinessObject.models.Dto.CategoryDTO;
using BusinessObject.models.Dto.ProductDto;
using eStore.Models;
using eStore.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;

namespace eStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductService _ProductService;
        //category service
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        public HomeController(IProductService productService, IMapper mapper, ICategoryService categoryService)
        {
            _ProductService = productService;
            _mapper = mapper;
            _categoryService = categoryService;


        }

        //load category list method
        public async Task LoadCategoryList(int selectedCategoryId)
        {
            var response = await _categoryService.GetAllAsync<APIResponse>();
            if (response != null && response.IsSuccess)
            {
                List<CategoryDTO> list = JsonConvert.DeserializeObject<List<CategoryDTO>>(Convert.ToString(response.Result));
                ViewBag.CategoryList = list.Select(i => new SelectListItem
                {
                    Text = i.CategoryName,
                    Value = i.CategoryId.ToString(),
                    Selected = i.CategoryId == selectedCategoryId
                });
            }
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            //Bình thường sẽ gọi DBContext để làm việc, nhưng giờ đang làm việc với API nên sẽ gọi Service API 
            List<ProductDTO> list = new();

            //var response = await _ProductService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            var response = await _ProductService.GetAllAsync<APIResponse>();
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<ProductDTO>>(Convert.ToString(response.Result));
            }
            return View(list);
        }

        [HttpPost]
        public async Task<IActionResult> Search(string search, int? pageSize = 0, int? pageNumber = 1)
        {
            List<ProductDTO> list = new();

            var response = await _ProductService.GetAllAsync<APIResponse>(search, pageSize, pageNumber);
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<ProductDTO>>(Convert.ToString(response.Result));
            }
            return View("Index",list);
        }
    }
}
