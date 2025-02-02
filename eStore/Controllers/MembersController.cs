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
using BusinessObject.models.Dto.MemberDTO;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using BusinessObject.models;

namespace eStore.Controllers
{
    public class MembersController : Controller
    {
        private readonly IMemberService _MemberService;
        private readonly IMapper _mapper;

        public MembersController(IMapper mapper, IMemberService MemberService)
        {
            _mapper = mapper;
            _MemberService = MemberService;


        }

        // GET: Members
        [Authorize(Roles = "admin")]

        public async Task<IActionResult> Index()
        {
            //Bình thường sẽ gọi DBContext để làm việc, nhưng giờ đang làm việc với API nên sẽ gọi Service API 
            List<MemberDTO> list = new();

            //var response = await _MemberService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.AccessToken));
            var response = await _MemberService.GetAllAsync<APIResponse>();
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<MemberDTO>>(Convert.ToString(response.Result));
            }
            return View(list);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create()
        {
            //viewbag rolelist customer and admin
            List<SelectListItem> roleList = new List<SelectListItem>();
            roleList.Add(new SelectListItem { Text = "Customer", Value = "customer" });
            roleList.Add(new SelectListItem { Text = "Admin", Value = "admin" });
            ViewBag.RoleList = roleList;
            return View();
        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MemberCreateDTO model)
        {
            if (ModelState.IsValid)
            {

                //var response = await _MemberService.CreateAsync<APIResponse>(model, HttpContext.Session.GetString(SD.AccessToken));
                var response = await _MemberService.CreateAsync<APIResponse>(model);
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Member created successfully";
                    return RedirectToAction(nameof(Index));
                }
            }
            TempData["error"] = "Error encountered.";
            return View(model);
        }
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int MemberId)
        {

            //var response = await _MemberService.GetAsync<APIResponse>(MemberId, HttpContext.Session.GetString(SD.AccessToken));
            var response = await _MemberService.GetAsync<APIResponse>(MemberId);
            if (response != null && response.IsSuccess)
            {

                MemberDTO model = JsonConvert.DeserializeObject<MemberDTO>(Convert.ToString(response.Result));

                return View(_mapper.Map<MemberUpdateDTO>(model));
            }
            return NotFound();
        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MemberUpdateDTO model)
        {
            if (ModelState.IsValid)
            {
                TempData["success"] = "Member updated successfully";
                //var response = await _MemberService.UpdateAsync<APIResponse>(model, HttpContext.Session.GetString(SD.AccessToken));
                var response = await _MemberService.UpdateAsync<APIResponse>(model);
                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            TempData["error"] = "Error encountered.";
            return View(model);
        }
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int MemberId)
        {
            //var response = await _MemberService.GetAsync<APIResponse>(MemberId, HttpContext.Session.GetString(SD.AccessToken));
            var response = await _MemberService.GetAsync<APIResponse>(MemberId);
            if (response != null && response.IsSuccess)
            {
                MemberDTO model = JsonConvert.DeserializeObject<MemberDTO>(Convert.ToString(response.Result));
                return View(model);
            }
            return NotFound();
        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(MemberDTO model)
        {

            //var response = await _MemberService.DeleteAsync<APIResponse>(model.MemberId, HttpContext.Session.GetString(SD.AccessToken));
            var response = await _MemberService.DeleteAsync<APIResponse>(model.MemberId);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Member deleted successfully";
                return RedirectToAction(nameof(Index));
            }
            TempData["error"] = "Error encountered.";
            return View(model);
        }


    }
}
