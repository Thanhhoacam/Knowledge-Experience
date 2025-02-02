using AutoMapper;
using Azure;
using BusinessObject.models;
using BusinessObject.models.Dto.CategoryDTO;
using BusinessObject.models.Dto.ProductDto;
using eStore.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace eStore.Controllers
{
	public class ProductsController : Controller
    {
        private readonly IProductService _ProductService;
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        public ProductsController(IProductService productService, IMapper mapper, ICategoryService categoryService)
        {
            _ProductService = productService;
            _mapper = mapper;
            _categoryService = categoryService;


		}

		//load category list method
		public async Task LoadCategoryList(int selectedId)
        {
            var response = await _categoryService.GetAllAsync<APIResponse>();
            if (response != null && response.IsSuccess)
            {
                List<CategoryDTO> list = JsonConvert.DeserializeObject<List<CategoryDTO>>(Convert.ToString(response.Result));
                ViewBag.CategoryList = list.Select(i => new SelectListItem
                {
                    Text = i.CategoryName,
                    Value = i.CategoryId.ToString(),
                    Selected = i.CategoryId == selectedId
				});
            }
        }

        // GET: Products
        //[Authorize(Roles = "admin")]

        public async Task<IActionResult> Index()
        {
            //Bình thường sẽ gọi DBContext để làm việc, nhưng giờ đang làm việc với API nên sẽ gọi Service API 
            List<ProductDTO> list = new();

            //var response = await _ProductService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.AccessToken));
            var response = await _ProductService.GetAllAsync<APIResponse>();
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<ProductDTO>>(Convert.ToString(response.Result));
            }
            return View(list);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create()
        {
            //call category API get all category
            await LoadCategoryList(-1);

            return View();
        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductCreateDTO model)
        {
            if (ModelState.IsValid)
            {

                //var response = await _ProductService.CreateAsync<APIResponse>(model, HttpContext.Session.GetString(SD.AccessToken));
                var response = await _ProductService.CreateAsync<APIResponse>(model);
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Product created successfully";
                    return RedirectToAction(nameof(Index));
                }
            }
            TempData["error"] = "Error encountered.";
            await LoadCategoryList(-1);
            return View(model);
        }
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int ProductId)
        {

            //var response = await _ProductService.GetAsync<APIResponse>(ProductId, HttpContext.Session.GetString(SD.AccessToken));
            var response = await _ProductService.GetAsync<APIResponse>(ProductId);
            if (response != null && response.IsSuccess)
            {

                ProductDTO model = JsonConvert.DeserializeObject<ProductDTO>(Convert.ToString(response.Result));
                await LoadCategoryList(model.CategoryId);

                return View(_mapper.Map<ProductUpdateDTO>(model));
            }
            return NotFound();
        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductUpdateDTO model)
        {
            if (ModelState.IsValid)
            {
                TempData["success"] = "Product updated successfully";
                //var response = await _ProductService.UpdateAsync<APIResponse>(model, HttpContext.Session.GetString(SD.AccessToken));
                var response = await _ProductService.UpdateAsync<APIResponse>(model);
                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            TempData["error"] = "Error encountered.";
			await LoadCategoryList(model.CategoryId);

			return View(model);
        }
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int ProductId)
        {
            //var response = await _ProductService.GetAsync<APIResponse>(ProductId, HttpContext.Session.GetString(SD.AccessToken));
            var response = await _ProductService.GetAsync<APIResponse>(ProductId);
            if (response != null && response.IsSuccess)
            {
                ProductDTO model = JsonConvert.DeserializeObject<ProductDTO>(Convert.ToString(response.Result));
                return View(model);
            }
            return NotFound();
        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(ProductDTO model)
        {

            //var response = await _ProductService.DeleteAsync<APIResponse>(model.ProductId, HttpContext.Session.GetString(SD.AccessToken));
            var response = await _ProductService.DeleteAsync<APIResponse>(model.ProductId);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Product deleted successfully";
                return RedirectToAction(nameof(Index));
            }
            TempData["error"] = "Error encountered.";
            return View(model);
        }
    }
}
