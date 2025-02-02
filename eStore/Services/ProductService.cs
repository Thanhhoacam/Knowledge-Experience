using BusinessObject.models.Dto.ProductDto;
using eStore.Models;
using eStore.Services.IServices;
using Store_Utility;

namespace eStore.Services
{
    public class ProductService : IProductService
    {
        private readonly IHttpClientFactory _clientFactory;
        private string storeUrl;
        private readonly IBaseService _baseService;

        public ProductService(IHttpClientFactory clientFactory, IConfiguration configuration, IBaseService baseService)
        {
            _clientFactory = clientFactory;
            storeUrl = configuration.GetValue<string>("ServiceUrls:StoreAPI");
            _baseService = baseService;
        }

        public async Task<T> CreateAsync<T>(ProductCreateDTO dto, string token)
        {
            return await _baseService.SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = dto,
                Url = storeUrl + "/api/productAPI",
                Token = token
            });
        }

        public async Task<T> DeleteAsync<T>(int id, string token)
        {
            return await _baseService.SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.DELETE,
                Url = storeUrl + "/api/productAPI/" + id,
                Token = token
            });
        }

        public async Task<T> GetAsync<T>(int id, string token)
        {
            return await _baseService.SendAsync<T> (new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = storeUrl + "/api/productAPI/" + id,
                Token = token
            });
        }
        public async Task<T> UpdateAsync<T>(ProductUpdateDTO dto, string token)
        {
            return await _baseService.SendAsync<T> (new APIRequest()
            {
                ApiType = SD.ApiType.PUT,
                Data = dto,
                Url = storeUrl + "/api/productAPI/" + dto.ProductId,
                Token = token
            });
        }

        //no session token

        public async Task<T> GetAsync<T>(int id)
        {
            return await _baseService.SendAsync<T> (new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = storeUrl + "/api/productAPI/" + id,
            });
        }

        public async Task<T> DeleteAsync<T>(int id)
        {
            return await _baseService.SendAsync<T> (new APIRequest()
            {
                ApiType = SD.ApiType.DELETE,
                Url = storeUrl + "/api/productAPI/" + id,
            });
        }

        public async Task<T> CreateAsync<T>(ProductCreateDTO dto)
        {
            return await _baseService.SendAsync<T> (new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = dto,
                Url = storeUrl + "/api/productAPI",
            });
        }

        public async Task<T> GetAllAsync<T>()
        {
            return await _baseService.SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = storeUrl + "/api/productAPI",
              

            });
        }
        public async Task<T> GetAllAsync<T>(string search, int? pagesize = 0, int? pagenumber = 1)
        {
            return await _baseService.SendAsync<T> (new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = storeUrl + "/api/productAPI",
                Search = search,
                PageSize = (int)pagesize,
                PageNumber = (int)pagenumber

            });
        }

        public async Task<T> UpdateAsync<T>(ProductUpdateDTO dto)
        {
            return await _baseService.SendAsync<T> (new APIRequest()
            {
                ApiType = SD.ApiType.PUT,
                Data = dto,
                Url = storeUrl + "/api/productAPI/" + dto.ProductId,
            });
        }
    }

}
