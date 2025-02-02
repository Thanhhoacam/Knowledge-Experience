using Azure;
using BusinessObject.models;
using BusinessObject.models.Dto.OrderDetailDTO;
using BusinessObject.models.Dto.OrderDTO;
using BusinessObject.models.Dto.ProductDto;
using eStore.Services;
using eStore.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Net.payOS;
using Net.payOS.Types;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace eStore.Controllers
{
	public class PaymentController : Controller
	{
		//orderservice
		private readonly IOrderService _orderService;
		//orderdetailservice
		private readonly IOrderDetailService _orderDetailService;
		//payos
		//productservice
		private readonly IProductService _productService;
		private readonly PayOS _payOS;
        private readonly IHttpContextAccessor _httpContextAccessor;
		//discount value intialize
		int discountV = 0;

        public PaymentController(IOrderService orderService, IOrderDetailService orderDetailService, PayOS payOS, IProductService productService, IHttpContextAccessor httpContextAccessor)
        {
            _orderService = orderService;
            _orderDetailService = orderDetailService;
            _payOS = payOS;
            _productService = productService;
            _httpContextAccessor = httpContextAccessor;
        }

        // GET: PaymentController1
        public async Task<ActionResult> Index([FromQuery]int ProductId)
		{
			//create product
			ProductDTO product = new ProductDTO();
			//get product by id
			var response = await _productService.GetAsync<APIResponse>(ProductId);
            if (response != null && response.IsSuccess)
            {
                product = JsonConvert.DeserializeObject<ProductDTO>(Convert.ToString(response.Result));
            }

            //return view name "CreateLink" with product
            return View("CreateLink", product);
		}

        [HttpPost("/create-payment-link")]
        public async Task<IActionResult> Checkout(int Quantity, string ProductName, decimal UnitPrice, string DiscountCode)
        {
			// check discount code is format Thanhxx xx is number
			if (DiscountCode != null && DiscountCode.Substring(0, 5) == "Thanh")
			{
				//cut xx from discount code and check it is number
				string discount = DiscountCode.Substring(5);
				if (int.TryParse(discount, out int discountValue))
				{
					   //check discount value is valid
					   if (discountValue > 0 && discountValue <= 100)
					{
						//discount price
						UnitPrice = UnitPrice - (UnitPrice * discountValue / 100);
					}
				}
                Console.WriteLine("Discount value: " + discountValue);
                discountV = discountValue;


            }

            try
            {
				int price = Convert.ToInt32(UnitPrice);
                int orderCode = int.Parse(DateTimeOffset.Now.ToString("ffffff"));
                ItemData item = new ItemData(ProductName.Trim(), Quantity, price);
                List<ItemData> items = new List<ItemData> { item };

                // Get the current request's base URL
                var request = _httpContextAccessor.HttpContext.Request;
                var baseUrl = $"{request.Scheme}://{request.Host}";

                PaymentData paymentData = new PaymentData(
                    orderCode,
                    2000,
                    "Thanh toan don hang ",
                    items,
                    $"{baseUrl}/cancel",
                    $"{baseUrl}/success?quantity={Quantity}&unitPrice={UnitPrice}&discount={discountV}"
                );

                CreatePaymentResult createPayment = await _payOS.createPaymentLink(paymentData);

                return Redirect(createPayment.checkoutUrl);
            }
            catch (System.Exception exception)
            {
                Console.WriteLine(exception);
                return Redirect("/");
            }
        }
        [HttpGet("/cancel")]
        public IActionResult Cancel()
        {
			TempData["error"] = "Payment Fail !";
            // Trả về trang HTML có tên "MyView.cshtml"
            return View("cancel");
        }
        [HttpGet("/success")]
        public async Task<ActionResult<APIResponse>> Success(int quantity, int unitPrice, int discount)
        {
			//create order
			OrderCreateDTO order = new OrderCreateDTO
			{
				OrderDate = DateTime.Now,
                RequiredDate = DateTime.Now.AddDays(7),
                ShippedDate = DateTime.Now.AddDays(2),
                Freight = 0, // free ship
            };

			//get order id
			var response = await _orderService.CreateAsync<APIResponse>(order);

            if (response != null && response.Result != null)
            {
                //get order id
				var orderDTO = JsonConvert.DeserializeObject<OrderDTO>(Convert.ToString(response.Result));

				//get product id
                int ProductId = 1;
                //get product by id
                var product = await _productService.GetAsync<ProductDTO>(ProductId);

                //create order detail
                OrderDetailCreateDTO orderDetail = new OrderDetailCreateDTO
				{
                    OrderId = orderDTO.OrderId,
                    ProductId = ProductId,
                    UnitPrice = unitPrice,
                    Quantity = quantity,
                    Discount = double.Parse(discount.ToString())
                };

                //create order detail
                var responseOrderDetail = await _orderDetailService.CreateAsync<APIResponse>(orderDetail);

                if (responseOrderDetail != null && responseOrderDetail.Result != null)
				{
					TempData["success"] = "Payment Success !";
                    return View("success");
				}
				else
				{
					TempData["success"] = "Payment Success But Error with Order & Orderdetails";

                }
            }


            TempData["success"] = "Payment Success !";
            // Trả về trang HTML có tên "MyView.cshtml"
            return View("success");
        }

        #region API CALLS
        // GET: PaymentController1/Details/5
        public ActionResult Details(int id)
		{
			return View();
		}

		// GET: PaymentController1/Create
		public ActionResult Create()
		{
			return View();
		}

		// POST: PaymentController1/Create
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

		// GET: PaymentController1/Edit/5
		public ActionResult Edit(int id)
		{
			return View();
		}

		// POST: PaymentController1/Edit/5
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

		// GET: PaymentController1/Delete/5
		public ActionResult Delete(int id)
		{
			return View();
		}

		// POST: PaymentController1/Delete/5
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
		#endregion
	}
}
