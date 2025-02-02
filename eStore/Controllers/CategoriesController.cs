using AutoMapper;
using BusinessObject.models;
using BusinessObject.models.Dto.CategoryDTO;
using eStore.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Store_Utility;

namespace eStore.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ICategoryService _CategoryService;
        private readonly IMapper _mapper;

        public CategoriesController(IMapper mapper, ICategoryService categoryService)
        {
            _mapper = mapper;
            _CategoryService = categoryService;


        }

        // GET: Categorys
        [Authorize(Roles = "admin")]

        public async Task<IActionResult> Index()
        {
            //Bình thường sẽ gọi DBContext để làm việc, nhưng giờ đang làm việc với API nên sẽ gọi Service API 
            List<CategoryDTO> list = new();

            //var response = await _CategoryService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.AccessToken));
            var response = await _CategoryService.GetAllAsync<APIResponse>();
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<CategoryDTO>>(Convert.ToString(response.Result));
            }
            return View(list);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create()
        {

            return View();
        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryCreateDTO model)
        {
            if (ModelState.IsValid)
            {

                //var response = await _CategoryService.CreateAsync<APIResponse>(model, HttpContext.Session.GetString(SD.AccessToken));
                var response = await _CategoryService.CreateAsync<APIResponse>(model);
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Category created successfully";
                    return RedirectToAction(nameof(Index));
                }
            }
            TempData["error"] = "Error encountered.";
            return View(model);
        }
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int CategoryId)
        {

            //var response = await _CategoryService.GetAsync<APIResponse>(CategoryId, HttpContext.Session.GetString(SD.AccessToken));
            var response = await _CategoryService.GetAsync<APIResponse>(CategoryId);
            if (response != null && response.IsSuccess)
            {

                CategoryDTO model = JsonConvert.DeserializeObject<CategoryDTO>(Convert.ToString(response.Result));

                return View(_mapper.Map<CategoryUpdateDTO>(model));
            }
            return NotFound();
        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CategoryUpdateDTO model)
        {
            if (ModelState.IsValid)
            {
                TempData["success"] = "Category updated successfully";
                //var response = await _CategoryService.UpdateAsync<APIResponse>(model, HttpContext.Session.GetString(SD.AccessToken));
                var response = await _CategoryService.UpdateAsync<APIResponse>(model);
                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            TempData["error"] = "Error encountered.";
            return View(model);
        }
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int CategoryId)
        {
            //var response = await _CategoryService.GetAsync<APIResponse>(CategoryId, HttpContext.Session.GetString(SD.AccessToken));
            var response = await _CategoryService.GetAsync<APIResponse>(CategoryId);
            if (response != null && response.IsSuccess)
            {
                CategoryDTO model = JsonConvert.DeserializeObject<CategoryDTO>(Convert.ToString(response.Result));
                return View(model);
            }
            return NotFound();
        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(CategoryDTO model)
        {

            //var response = await _CategoryService.DeleteAsync<APIResponse>(model.CategoryId, HttpContext.Session.GetString(SD.AccessToken));
            var response = await _CategoryService.DeleteAsync<APIResponse>(model.CategoryId);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Category deleted successfully";
                return RedirectToAction(nameof(Index));
            }
            TempData["error"] = "Error encountered.";
            return View(model);
        }
    }
}
