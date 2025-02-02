using BusinessObject.models.Dto.CategoryDTO;
using eStore.Models;
using eStore.Services.IServices;
using Store_Utility;

namespace eStore.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IHttpClientFactory _clientFactory;
        private string storeUrl;
        private readonly IBaseService _baseService;

        public CategoryService(IHttpClientFactory clientFactory, IConfiguration configuration, IBaseService baseService)
        {
            _clientFactory = clientFactory;
            storeUrl = configuration.GetValue<string>("ServiceUrls:StoreAPI");
            _baseService = baseService;
        }

        public async Task<T> CreateAsync<T>(CategoryCreateDTO dto)
        {
            return await _baseService.SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = dto,
                Url = storeUrl + "/api/CategoryAPI",
            });
        }

        public async Task<T> DeleteAsync<T>(int id)
        {
            return await _baseService.SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.DELETE,
                Url = storeUrl + "/api/CategoryAPI/" + id,
            });
        }

       

        public async Task<T> GetAllAsync<T>()
        {
            return await _baseService.SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = storeUrl + "/api/CategoryAPI",

            });
        }

        public async Task<T> GetAsync<T>(int id)
        {
            return await _baseService.SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = storeUrl + "/api/CategoryAPI/" + id,
            });
        }

        public async Task<T> UpdateAsync<T>(CategoryUpdateDTO dto)
        {
            return await _baseService.SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.PUT,
                Data = dto,
                Url = storeUrl + "/api/CategoryAPI/" + dto.CategoryId,
            });
        }

    }
}
