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
using BusinessObject.models.Dto.OrderDTO;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using BusinessObject.models;
using BusinessObject.models.Dto.MemberDTO;

namespace eStore.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IOrderService _OrderService;
        //orderdetailservice
        private readonly IOrderDetailService _OrderDetailService;
        //memberservice
        private readonly IMemberService _memberService;
        private readonly IMapper _mapper;

		public OrdersController(IMapper mapper, IOrderService OrderService, IOrderDetailService orderDetailService, IMemberService memberService)
		{
			_mapper = mapper;
			_OrderService = OrderService;
			_OrderDetailService = orderDetailService;
			_memberService = memberService;
		}

		public async Task LoadMemberList(int selectedId)
		{
			var response = await _memberService.GetAllAsync<APIResponse>();
			if (response != null && response.IsSuccess)
			{
				List<MemberDTO> list = JsonConvert.DeserializeObject<List<MemberDTO>>(Convert.ToString(response.Result));
				ViewBag.MemberList = list.Select(i => new SelectListItem
				{
					Text = i.Email,
					Value = i.MemberId.ToString(),
					Selected = i.MemberId == selectedId
				});
			}
		}


		// GET: Orders
		[Authorize(Roles = "admin")]

        public async Task<IActionResult> Index()
        {
            //Bình thường sẽ gọi DBContext để làm việc, nhưng giờ đang làm việc với API nên sẽ gọi Service API 
            List<OrderDTO> list = new();

            //var response = await _OrderService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.AccessToken));
            var response = await _OrderService.GetAllAsync<APIResponse>();
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<OrderDTO>>(Convert.ToString(response.Result));
            }
            return View(list);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create()
        {
			await LoadMemberList(-1);

			return View();
        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OrderCreateDTO model)
        {

			if (ModelState.IsValid)
            {

                //var response = await _OrderService.CreateAsync<APIResponse>(model, HttpContext.Session.GetString(SD.AccessToken));
                var response = await _OrderService.CreateAsync<APIResponse>(model);
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Order created successfully";
                    return RedirectToAction(nameof(Index));
                }
            }
            TempData["error"] = "Error encountered.";
			await LoadMemberList(-1);

			return View(model);
        }


        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int OrderId)
        {

			//var response = await _OrderService.GetAsync<APIResponse>(OrderId, HttpContext.Session.GetString(SD.AccessToken));
			var response = await _OrderService.GetAsync<APIResponse>(OrderId);
            if (response != null && response.IsSuccess)
            {

                OrderDTO model = JsonConvert.DeserializeObject<OrderDTO>(Convert.ToString(response.Result));
				await LoadMemberList(model.MemberId);

				return View(_mapper.Map<OrderUpdateDTO>(model));
            }
            return NotFound();
        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(OrderUpdateDTO model)
        {
            if (ModelState.IsValid)
            {
                TempData["success"] = "Order updated successfully";
                //var response = await _OrderService.UpdateAsync<APIResponse>(model, HttpContext.Session.GetString(SD.AccessToken));
                var response = await _OrderService.UpdateAsync<APIResponse>(model);
                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            TempData["error"] = "Error encountered.";
			await LoadMemberList(model.MemberId);

			return View(model);
        }
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int OrderId)
        {
            //var response = await _OrderService.GetAsync<APIResponse>(OrderId, HttpContext.Session.GetString(SD.AccessToken));
            var response = await _OrderService.GetAsync<APIResponse>(OrderId);
            if (response != null && response.IsSuccess)
            {
                OrderDTO model = JsonConvert.DeserializeObject<OrderDTO>(Convert.ToString(response.Result));
                
                return View(model);


            }
            return NotFound();
        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(OrderDTO model)
        {

            //var response = await _OrderService.DeleteAsync<APIResponse>(model.OrderId, HttpContext.Session.GetString(SD.AccessToken));
            var response = await _OrderService.DeleteAsync<APIResponse>(model.OrderId);
            var orderDetailResponse = await _OrderDetailService.DeleteAsync<APIResponse>(model.OrderId);
            if (response != null && response.IsSuccess)
            {
                if (orderDetailResponse != null && orderDetailResponse.IsSuccess)
                {
                TempData["success"] = "Order and Order details deleted successfully";
                return RedirectToAction(nameof(Index));
                }
                else
                {
					TempData["success"] = "Only Order deleted successfully";
					return RedirectToAction(nameof(Index));
				}
				
			}
            TempData["error"] = "Error encountered.";
            return View(model);
        }


    }
}
