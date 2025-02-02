using BusinessObject.models.Dto.OrderDTO;
using eStore.Models;
using eStore.Services.IServices;
using Store_Utility;

namespace eStore.Services
{
    public class OrderService : IOrderService
    {
        private readonly IHttpClientFactory _clientFactory;
        private string storeUrl;
        private readonly IBaseService _baseService;


        public OrderService(IHttpClientFactory clientFactory, IConfiguration configuration, IBaseService baseService)
        {
            _clientFactory = clientFactory;
            storeUrl = configuration.GetValue<string>("ServiceUrls:StoreAPI");
            _baseService = baseService;
        }

        public async Task<T> CreateAsync<T>(OrderCreateDTO dto)
        {
            return await  _baseService.SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = dto,
                Url = storeUrl + "/api/OrderAPI",
                
            });
        }

        public async Task<T> DeleteAsync<T>(int id)
        {
           return await  _baseService.SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.DELETE,
                Url = storeUrl + "/api/OrderAPI/" + id,
                
            });
        }


        public async Task<T> GetAllAsync<T>()
        {
           return await  _baseService.SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = storeUrl + "/api/OrderAPI",

            });
        }

        public async Task<T> GetAsync<T>(int id)
        {
           return await  _baseService.SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = storeUrl + "/api/OrderAPI/" + id,
                
            });
        }

        public async Task<T> UpdateAsync<T>(OrderUpdateDTO dto)
        {
           return await  _baseService.SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.PUT,
                Data = dto,
                Url = storeUrl + "/api/OrderAPI/" + dto.OrderId,
                
            });
        }

    }
}
