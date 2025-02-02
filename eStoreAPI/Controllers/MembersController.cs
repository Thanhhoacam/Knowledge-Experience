using AutoMapper;
using BusinessObject.models;
using BusinessObject.models.Dto.LoginDTO;
using BusinessObject.models.Dto.MemberDTO;
using BusinessObject.models.Dto.RegisterDTO;
using BusinessObject.Object;
using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text.Json;

namespace eStoreAPI.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/MemberAPI")]
    [ApiController]
    public class MembersController : ControllerBase
    {
        private readonly IMemberRepository _MemberRepository;
        protected APIResponse _response;
        private readonly IMapper _mapper;

        public MembersController(IMemberRepository MemberRepository, IMapper mapper)
        {
            _MemberRepository = MemberRepository;
            _response = new();
            _mapper = mapper;
        }

        // GET: api/Members
        [HttpGet]
        //[ResponseCache(CacheProfileName = "Default30")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetMembers([FromQuery] string? search, int pageSize = 0, int pageNumber = 1)
        {
            try
            {
                IEnumerable<Member> MemberList;

                MemberList = await _MemberRepository.GetAllAsync(pageSize: pageSize,
                    pageNumber: pageNumber);

                if (!string.IsNullOrEmpty(search))
                {
                    MemberList = MemberList.Where(u => u.Email.ToLower().Contains(search));
                }
                Pagination pagination = new() { PageNumber = pageNumber, PageSize = pageSize };

                Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagination));
                _response.Result = _mapper.Map<List<MemberDTO>>(MemberList);
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

        // GET: api/MemberAPI/5
        [HttpGet("{id:int}", Name = "GetMember")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetMember(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var Member = await _MemberRepository.GetAsync(u => u.MemberId == id);
                if (Member == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                _response.Result = _mapper.Map<MemberDTO>(Member);
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

        // PUT: api/MemberAPI/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id:int}", Name = "UpdateMember")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> UpdateMember(int id, [FromBody] MemberUpdateDTO updateDTO)
        {
            try
            {
                if (updateDTO == null || id != updateDTO.MemberId)
                {
                    return BadRequest();
                }

                Member model = _mapper.Map<Member>(updateDTO);

                await _MemberRepository.UpdateAsync(model);
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                _response.Result = _mapper.Map<MemberDTO>(model);
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

        // POST: api/MemberAPI
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CreateMember([FromBody] MemberCreateDTO createDTO)
        {
            try
            {

                if (await _MemberRepository.GetAsync(u => u.Email.ToLower() == createDTO.Email.ToLower()) != null)
                {
                    ModelState.AddModelError("ErrorMessages", "Member already Exists!");
                    return BadRequest(ModelState);
                }

                if (createDTO == null)
                {
                    return BadRequest(createDTO);
                }

                Member Member = _mapper.Map<Member>(createDTO);


                await _MemberRepository.CreateAsync(Member);
                _response.Result = _mapper.Map<MemberDTO>(Member);
                _response.StatusCode = HttpStatusCode.Created;
                _response.IsSuccess = true;

                return CreatedAtRoute("GetMember", new { id = Member.MemberId }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                     = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        // DELETE: api/MemberAPI/5
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete("{id:int}", Name = "DeleteMember")]
        public async Task<ActionResult<APIResponse>> DeleteMember(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }
                var Member = await _MemberRepository.GetAsync(u => u.MemberId == id);
                if (Member == null)
                {
                    return NotFound();
                }
                await _MemberRepository.RemoveAsync(Member);
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

        //private bool MemberExists(int id)
        //{
        //    return (_context.Members?.Any(e => e.MemberId == id)).GetValueOrDefault();
        //}

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO model)
        {
            var loginResponse = await _MemberRepository.Login(model);
            if (loginResponse == null || string.IsNullOrEmpty(loginResponse.AccessToken))
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Username or password is incorrect");
                return BadRequest(_response);
            }
            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            _response.Result = loginResponse;
            return Ok(_response);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterationRequestDTO model)
        {
            bool ifUserNameUnique = _MemberRepository.IsUniqueUser(model.Email);
            if (!ifUserNameUnique)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Username already exists");
                return BadRequest(_response);
            }

            var user = await _MemberRepository.Register(model);
            if (user == null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Error while registering");
                return BadRequest(_response);
            }
            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            _response.Result = _mapper.Map<MemberDTO>(user);
            return Ok(_response);
        }
    }
}
