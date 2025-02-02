using BusinessObject.models;
using BusinessObject.models.Dto.CategoryDTO;
using BusinessObject.models.Dto.MemberDTO;
using BusinessObject.models.Dto.OrderDetailDTO;
using BusinessObject.models.Dto.OrderDTO;
using BusinessObject.models.Dto.ProductDto;
using eStore.Services;
using eStore.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace eStore.Controllers
{
    public class StatisticsController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IOrderDetailService _orderDetailService;
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IMemberService _memberService;
		public StatisticsController(IOrderService orderService, IOrderDetailService orderDetailService, IProductService productService, ICategoryService categoryService, IMemberService memberService)
		{
			_orderService = orderService;
			_orderDetailService = orderDetailService;
			_productService = productService;
			_categoryService = categoryService;
			_memberService = memberService;
		}
		// GET: StatisticsController
		public ActionResult Index()
        {
            return View();
        }
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Index(IFormCollection collection)
		{

			//var response = await _OrderDetailService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.AccessToken));
			var odResponse = await _orderDetailService.GetAllAsync<APIResponse>(); // get Revenue from unit price and quantity and discount from startDate to endDate
            var oResponse = await _orderService.GetAllAsync<APIResponse>(); // to get NumberOrder
            var pResponse = await _productService.GetAllAsync<APIResponse>(); // to get NumberProduct
            var cResponse = await _categoryService.GetAllAsync<APIResponse>(); // to get NumberCategory
            var mResponse = await _memberService.GetAllAsync<APIResponse>(); // to get NumberMember
			if (odResponse != null && oResponse !=null)
			{
				var olist = JsonConvert.DeserializeObject<List<OrderDTO>>(Convert.ToString(oResponse.Result));
				ViewBag.NumberOrder = olist.Count;
				//get list order from startDate to endDate 
                var dateOrderlist = olist.Where(x => x.OrderDate >= Convert.ToDateTime(collection["startDate"]) && x.OrderDate <= Convert.ToDateTime(collection["endDate"])).ToList();
				//get list orderDetail from dateOrderlist
				var odList = JsonConvert.DeserializeObject<List<OrderDetailDTO>>(Convert.ToString(odResponse.Result));
                var list = odList.Where(x => dateOrderlist.Any(y => y.OrderId == x.OrderId)).ToList();
                //get Revenue from unit price and quantity and discount %
                double Revenue = 0;
                foreach (var item in list)
                {
                    Revenue += (double)item.UnitPrice * item.Quantity * (1 - item.Discount);
				}
                ViewBag.Revenue = Revenue;

			}
            
			if (pResponse != null)
            {
				var list = JsonConvert.DeserializeObject<List<ProductDTO>>(Convert.ToString(pResponse.Result));
				ViewBag.NumberProduct = list.Count;
			}
			if (cResponse != null)
            {
				var list = JsonConvert.DeserializeObject<List<CategoryDTO>>(Convert.ToString(cResponse.Result));
				ViewBag.NumberCategory = list.Count;
			}
			if (mResponse != null)
            {
				var list = JsonConvert.DeserializeObject<List<MemberDTO>>(Convert.ToString(mResponse.Result));
				ViewBag.NumberMember = list.Count;
			}
			
				return View();
			
		}
		// GET: StatisticsController/Details/5
		public ActionResult Details(int id)
        {
            return View();
        }

        // GET: StatisticsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: StatisticsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: StatisticsController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: StatisticsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: StatisticsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: StatisticsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
