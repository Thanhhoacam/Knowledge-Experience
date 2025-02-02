using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BusinessObject;
using BusinessObject.Object;
using AutoMapper;
using eStore.Services.IServices;
using BusinessObject.models.Dto.OrderDetailDTO;
using eStore.Services;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using BusinessObject.models;
using BusinessObject.models.Dto.CategoryDTO;
using BusinessObject.models.Dto.ProductDto;
using BusinessObject.models.Dto.OrderDTO;
using Azure;

namespace eStore.Controllers
{
    public class OrderDetailsController : Controller
    {
        private readonly IOrderDetailService _OrderDetailService;
        private readonly IProductService _productService;
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public OrderDetailsController(IMapper mapper, IOrderDetailService OrderDetailService, IProductService productService, IOrderService orderService)
        {
            _mapper = mapper;
            _OrderDetailService = OrderDetailService;
            _productService = productService;
            _orderService = orderService;
        }

        public async Task LoadOrderProductList(int? selectedProductId = -1, int? selectedOrderId = -1)
        {
            var responseOrder = await _orderService.GetAllAsync<APIResponse>();
            var responseProduct = await _productService.GetAllAsync<APIResponse>();

            if (responseOrder != null && responseOrder.IsSuccess)
            {
                List<OrderDTO> list = JsonConvert.DeserializeObject<List<OrderDTO>>(Convert.ToString(responseOrder.Result));
                ViewBag.OrderList = list.Select(i => new SelectListItem
                {
                    Text = i.OrderId.ToString(),
                    Value = i.OrderId.ToString(),
                    Selected = i.OrderId == selectedOrderId
                });
            }

            if (responseProduct != null && responseProduct.IsSuccess)
            {
                List<ProductDTO> list = JsonConvert.DeserializeObject<List<ProductDTO>>(Convert.ToString(responseProduct.Result));
                ViewBag.ProductList = list.Select(i => new SelectListItem
                {
                    Text = i.ProductName,
                    Value = i.ProductId.ToString(),
                    Selected = i.ProductId == selectedOrderId
                });
            }
        }

        // GET: OrderDetails
        [Authorize(Roles = "admin")]

        public async Task<IActionResult> Index()
        {
            //Bình thường sẽ gọi DBContext để làm việc, nhưng giờ đang làm việc với API nên sẽ gọi Service API 
            List<OrderDetailDTO> list = new();

            //var response = await _OrderDetailService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.AccessToken));
            var response = await _OrderDetailService.GetAllAsync<APIResponse>();
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<OrderDetailDTO>>(Convert.ToString(response.Result));
                //sort list by order id
                list = list.OrderBy(x => x.OrderId).ToList();
            }
            return View(list);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create()
        {
           await LoadOrderProductList();
            

            return View();
        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OrderDetailCreateDTO model)
        {

            if (ModelState.IsValid)
            {

                //var response = await _OrderDetailService.CreateAsync<APIResponse>(model, HttpContext.Session.GetString(SD.AccessToken));
                var response = await _OrderDetailService.CreateAsync<APIResponse>(model);
                if (response != null && response.Result != null)
                {
                    TempData["success"] = "OrderDetail created successfully";
                    return RedirectToAction(nameof(Index));
                }else
                {
                TempData["error"] = "Error encountered: " + response.ErrorMessages.FirstOrDefault();
                }

            }
            await LoadOrderProductList();

            //TempData["error"] = "Error encountered.";
            return View(model);
        }
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int OrderId, int ProductId)
        {

            //var response = await _OrderDetailService.GetAsync<APIResponse>(OrderDetailId, HttpContext.Session.GetString(SD.AccessToken));
            var response = await _OrderDetailService.GetAsync<APIResponse>(OrderId, ProductId);
            if (response != null && response.IsSuccess)
            {

                OrderDetailDTO model = JsonConvert.DeserializeObject<OrderDetailDTO>(Convert.ToString(response.Result));
                await LoadOrderProductList(model.ProductId,model.OrderId);
                return View(_mapper.Map<OrderDetailUpdateDTO>(model));
            }
            return NotFound();
        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(OrderDetailUpdateDTO model)
        {
            if (ModelState.IsValid)
            {
                TempData["success"] = "OrderDetail updated successfully";
                //var response = await _OrderDetailService.UpdateAsync<APIResponse>(model, HttpContext.Session.GetString(SD.AccessToken));
                var response = await _OrderDetailService.UpdateAsync<APIResponse>(model);
                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            await LoadOrderProductList(model.ProductId, model.OrderId);

            TempData["error"] = "Error encountered.";
            return View(model);
        }
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int OrderId, int productId)
        {
            //var response = await _OrderDetailService.GetAsync<APIResponse>(OrderDetailId, HttpContext.Session.GetString(SD.AccessToken));
            var response = await _OrderDetailService.GetAsync<APIResponse>(OrderId, productId);
            if (response != null && response.IsSuccess)
            {
                OrderDetailDTO model = JsonConvert.DeserializeObject<OrderDetailDTO>(Convert.ToString(response.Result));
                return View(model);
            }
            return NotFound();
        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(OrderDetailDTO model)
        {

            //var response = await _OrderDetailService.DeleteAsync<APIResponse>(model.OrderDetailId, HttpContext.Session.GetString(SD.AccessToken));
            var response = await _OrderDetailService.DeleteEachAsync<APIResponse>(model.OrderId, model.ProductId);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "OrderEachDetail deleted successfully";
                return RedirectToAction(nameof(Index));
            }
            TempData["error"] = "Error encountered.";
            return View(model);
        }
    }
}
