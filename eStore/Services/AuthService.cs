using BusinessObject.models.Dto.LoginDTO;
using BusinessObject.models.Dto.RegisterDTO;
using eStore.Models;
using eStore.Services.IServices;
using Store_Utility;

namespace eStore.Services
{
    public class AuthService :  IAuthService
    {
        private readonly IHttpClientFactory _clientFactory;
        private string storeUrl;
        private readonly IBaseService _baseService;


        public AuthService(IHttpClientFactory clientFactory, IConfiguration configuration, IBaseService baseService)
        {
            _clientFactory = clientFactory;
            storeUrl = configuration.GetValue<string>("ServiceUrls:StoreAPI");
            _baseService = baseService;
        }

        public async Task<T> LoginAsync<T>(LoginRequestDTO obj)
        {
            return await  _baseService.SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = obj,
                Url = storeUrl + "/api/MemberAPI/login"
            },withBearer:false);
        }

         public async Task<T> RegisterAsync<T>(RegisterationRequestDTO obj)
        {
            return await  _baseService.SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = obj,
                Url = storeUrl + "/api/MemberAPI/register"
            }, withBearer: false);
        }

    }
}
