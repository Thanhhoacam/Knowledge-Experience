using AutoMapper;
using BusinessObject.models;
using BusinessObject.models.Dto.OrderDetailDTO;
using BusinessObject.Object;
using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using System.Net;
using System.Text.Json;

namespace eStoreAPI.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/OrderDetailAPI")]
    [ApiController]
    public class OrderDetailsController : ControllerBase
    {
        private readonly IOrderDetailRepository _OrderDetailRepository;
        protected APIResponse _response;
        private readonly IMapper _mapper;

        public OrderDetailsController(IOrderDetailRepository OrderDetailRepository, IMapper mapper)
        {
            _OrderDetailRepository = OrderDetailRepository;
            _response = new();
            _mapper = mapper;
        }

        // GET: api/OrderDetails
        [HttpGet]
        //[ResponseCache(CacheProfileName = "Default30")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetOrderDetails([FromQuery] string? search, int pageSize = 0, int pageNumber = 1)
        {
            try
            {
                IEnumerable<OrderDetail> OrderDetailList;

                OrderDetailList = await _OrderDetailRepository.GetAllAsync(pageSize: pageSize,
                    pageNumber: pageNumber, includeProperties: "Product");

                if (!string.IsNullOrEmpty(search))
                {
                    OrderDetailList = OrderDetailList.Where(u => u.UnitPrice.ToString().Contains(search) || u.Discount.ToString().Contains(search));
                }
                Pagination pagination = new() { PageNumber = pageNumber, PageSize = pageSize };

                Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagination));
                _response.Result = _mapper.Map<List<OrderDetailDTO>>(OrderDetailList);
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                     = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        // GET: api/OrderDetails/5
        [HttpGet("{orderId:int}/{productId:int}", Name = "GetOrderDetail")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetOrderDetail(int orderId, int productId)
        {
            try
            {
                if (orderId == 0 || productId == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var OrderDetail = await _OrderDetailRepository.GetAsync(u => u.OrderId == orderId && u.ProductId == productId, includeProperties: "Product");
                if (OrderDetail == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                _response.Result = _mapper.Map<OrderDetailDTO>(OrderDetail);
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                     = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        // PUT: api/OrderDetails/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{orderId:int}/{productId:int}", Name = "UpdateOrderDetail")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> UpdateOrderDetail(int orderId,int productId, [FromBody] OrderDetailUpdateDTO updateDTO)
        {
            try
            {
                if (updateDTO == null || orderId != updateDTO.OrderId || productId != updateDTO.ProductId)
                {
                    return BadRequest();
                }

                OrderDetail model = _mapper.Map<OrderDetail>(updateDTO);

                await _OrderDetailRepository.UpdateAsync(model);
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                     = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        // POST: api/OrderDetails
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CreateOrderDetail([FromBody] OrderDetailCreateDTO createDTO)
        {
            try
            {
                var res = (await _OrderDetailRepository.GetAsync(u => u.OrderId == createDTO.OrderId && u.ProductId == createDTO.ProductId));
                if (res != null)
                {
                    ModelState.AddModelError("ErrorMessages", "OrderDetail already Exists!");
                    return BadRequest(ModelState);
                }

                if (createDTO == null)
                {
                    return BadRequest(createDTO);
                }

                OrderDetail OrderDetail = _mapper.Map<OrderDetail>(createDTO);


                await _OrderDetailRepository.CreateAsync(OrderDetail);
                _response.Result = _mapper.Map<OrderDetailDTO>(OrderDetail);
                _response.StatusCode = HttpStatusCode.Created;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                     = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        // DELETE: api/OrderDetails/5
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete("{id:int}", Name = "DeleteOrderDetail")]
        public async Task<ActionResult<APIResponse>> DeleteOrderDetail(int id) //delete all order details for an order
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }
                var OrderDetail = await _OrderDetailRepository.GetAllAsync(u => u.OrderId == id);
                if (OrderDetail == null)
                {
                    return NotFound();
                }
                //for each



                await _OrderDetailRepository.RemoveAllAsync(OrderDetail);
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                     = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        // DELETE: api/OrderDetails/5
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete("{orderId:int}/{productId:int}", Name = "DeleteEachOrderDetail")]
        public async Task<ActionResult<APIResponse>> DeleteEachOrderDetail(int orderId, int productId) //delete all order details for an order
        {
            try
            {
                if (orderId == 0 || productId == 0)
                {
                    return BadRequest();
                }
                var OrderDetail = await _OrderDetailRepository.GetAsync(u => u.OrderId == orderId && u.ProductId == productId);
                if (OrderDetail == null)
                {
                    return NotFound();
                }

                await _OrderDetailRepository.RemoveAsync(OrderDetail);
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                     = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        //private bool OrderDetailExists(int id)
        //{
        //    return (_context.OrderDetails?.Any(e => e.OrderDetailId == id)).GetValueOrDefault();
        //}
    }
}
