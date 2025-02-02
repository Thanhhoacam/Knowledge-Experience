using AutoMapper;
using BusinessObject;
using BusinessObject.models.Dto.LoginDTO;
using BusinessObject.models.Dto.MemberDTO;
using BusinessObject.models.Dto.RegisterDTO;
using BusinessObject.Object;
using DataAccess.GenericDAO;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace DataAccess.Dao
{
    public class MemberDAOv2 : GenericDAO<Member>
    {
        private readonly ApplicationDbContext _db;
        private string secretKey;
        private readonly IMapper _mapper;
        public MemberDAOv2(ApplicationDbContext db, IConfiguration configuration, IMapper mapper) : base(db)
        {
            _db = db;
            secretKey = configuration["ApiSettings:Secret"];
            _mapper = mapper;
        }

        public bool IsUniqueUser(string email)
        {
            var user = _db.Members.FirstOrDefault(x => x.Email == email);
            if (user == null)
            {
                return true;
            }
            return false;
        }

        public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
        {
            var user = _db.Members
                .FirstOrDefault(u => u.Email.ToLower() == loginRequestDTO.Email.ToLower() && u.Password == loginRequestDTO.Password);


            if (user == null)
            {
                return new LoginResponseDTO()
                {
                    AccessToken = "",
                    //User = null
                };
            }
            //if user was found generate JWT Token
            var accessToken = await GetAccessToken(user);

            LoginResponseDTO loginResponseDTO = new LoginResponseDTO()
            {
                AccessToken = accessToken,

            };
            return loginResponseDTO;
        }
        private async Task<string> GetAccessToken(Member user)
        {
            //if user was found generate JWT Token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Email, user.Email.ToString()),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenStr = tokenHandler.WriteToken(token);
            return tokenStr;
        }

        public async Task<Member> Register(RegisterationRequestDTO registerationRequestDTO)
        {
            Member user = new()
            {
                Email = registerationRequestDTO.Email,
                Password = registerationRequestDTO.Password,
                Role = registerationRequestDTO.Role
            };

           _db.Members.Add(user);
            await _db.SaveChangesAsync();
            user.Password = "";
            return user;
        }

        public async Task<Member> UpdateAsync(Member entity)
        {
            _db.Members.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }



    }

}
