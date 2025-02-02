using BusinessObject.models.Dto.MemberDTO;
using eStore.Models;
using eStore.Services.IServices;
using Store_Utility;

namespace eStore.Services
{
    public class MemberService : IMemberService
    {
        private readonly IHttpClientFactory _clientFactory;
        private string storeUrl;
        private readonly IBaseService _baseService;


        public MemberService(IHttpClientFactory clientFactory, IConfiguration configuration, IBaseService baseService)
        {
            _clientFactory = clientFactory;
            storeUrl = configuration.GetValue<string>("ServiceUrls:StoreAPI");
            _baseService = baseService;
        }

        public async Task<T> CreateAsync<T>(MemberCreateDTO dto)
        {
            return await  _baseService.SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = dto,
                Url = storeUrl + "/api/MemberAPI",
                
            });
        }

        public async Task<T> DeleteAsync<T>(int id)
        {
            return await  _baseService.SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.DELETE,
                Url = storeUrl + "/api/MemberAPI/" + id,
                
            });
        }


        public async Task<T> GetAllAsync<T>()
        {
            return await  _baseService.SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = storeUrl + "/api/MemberAPI",

            });
        }

        public async Task<T> GetAsync<T>(int id)
        {
            return await  _baseService.SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = storeUrl + "/api/MemberAPI/" + id,
                
            });
        }

        public async Task<T> UpdateAsync<T>(MemberUpdateDTO dto)
        {
            return await  _baseService.SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.PUT,
                Data = dto,
                Url = storeUrl + "/api/MemberAPI/" + dto.MemberId,
                
            });
        }

    }
}
