using AutoMapper;
using BusinessObject.models;
using BusinessObject.models.Dto.OrderDTO;
using BusinessObject.Object;
using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text.Json;

namespace eStoreAPI.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/OrderAPI")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderRepository _OrderRepository;
        protected APIResponse _response;
        private readonly IMapper _mapper;

        public OrdersController(IOrderRepository OrderRepository, IMapper mapper)
        {
            _OrderRepository = OrderRepository;
            _response = new();
            _mapper = mapper;
        }

        // GET: api/Orders
        [HttpGet]
        //[ResponseCache(CacheProfileName = "Default30")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetOrders([FromQuery] string? search, int pageSize = 0, int pageNumber = 1)
        {
            try
            {
                IEnumerable<Order> OrderList;

                OrderList = await _OrderRepository.GetAllAsync(pageSize: pageSize,
                    pageNumber: pageNumber, includeProperties: "OrderDetails,Member");

                if (!string.IsNullOrEmpty(search))
                {
                    OrderList = OrderList.Where(u => u.OrderDate.ToString().ToLower().Contains(search));
                }
                Pagination pagination = new() { PageNumber = pageNumber, PageSize = pageSize };

                Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagination));
                _response.Result = _mapper.Map<List<OrderDTO>>(OrderList);
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

        // GET: api/Orders/5
        [HttpGet("{id:int}", Name = "GetOrder")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetOrder(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var Order = await _OrderRepository.GetAsync(u => u.OrderId == id
                    );
                if (Order == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                _response.Result = _mapper.Map<OrderDTO>(Order);
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

        // PUT: api/Orders/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id:int}", Name = "UpdateOrder")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> UpdateOrder(int id, [FromBody] OrderUpdateDTO updateDTO)
        {
            try
            {
                if (updateDTO == null || id != updateDTO.OrderId || updateDTO.MemberId == 0 || updateDTO.ShippedDate <= updateDTO.RequiredDate)
                {
                    return BadRequest();
                }

                Order model = _mapper.Map<Order>(updateDTO);

                await _OrderRepository.UpdateAsync(model);
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

        // POST: api/Orders
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CreateOrder([FromBody] OrderCreateDTO createDTO)
        {
            try
            {

                //if (await _OrderRepository.GetAsync(u => u.OrderId == createDTO.OrderId) != null)
                //{
                //    ModelState.AddModelError("ErrorMessages", "Order already Exists!");
                //    return BadRequest(ModelState);
                //}

                if (createDTO == null || createDTO.MemberId == 0)
                {
                    return BadRequest(createDTO);
                }

                Order Order = _mapper.Map<Order>(createDTO);


                await _OrderRepository.CreateAsync(Order);
                _response.Result = _mapper.Map<OrderDTO>(Order);
                _response.StatusCode = HttpStatusCode.Created;
                _response.IsSuccess = true;

                return CreatedAtRoute("GetOrder", new { id = Order.OrderId }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                     = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        // DELETE: api/Orders/5
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete("{id:int}", Name = "DeleteOrder")]
        public async Task<ActionResult<APIResponse>> DeleteOrder(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }
                var Order = await _OrderRepository.GetAsync(u => u.OrderId == id);
                if (Order == null)
                {
                    return NotFound();
                }
                await _OrderRepository.RemoveAsync(Order);
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

        //private bool OrderExists(int id)
        //{
        //    return (_context.Orders?.Any(e => e.OrderId == id)).GetValueOrDefault();
        //}
    }
}
