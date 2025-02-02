using BusinessObject.models.Dto.OrderDetailDTO;
using eStore.Models;
using eStore.Services.IServices;
using Humanizer;
using Store_Utility;

namespace eStore.Services
{
    public class OrderDetailService : IOrderDetailService
    {
        private readonly IHttpClientFactory _clientFactory;
        private string storeUrl;

        private readonly IBaseService _baseService;


        public OrderDetailService(IHttpClientFactory clientFactory, IConfiguration configuration, IBaseService baseService)
        {
            _clientFactory = clientFactory;
            storeUrl = configuration.GetValue<string>("ServiceUrls:StoreAPI");
            _baseService = baseService;
        }

        public async Task<T> CreateAsync<T>(OrderDetailCreateDTO dto)
        {
            return await  _baseService.SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = dto,
                Url = storeUrl + "/api/orderDetailAPI",
                
            });
        }

        public async Task<T> DeleteAsync<T>(int orderId)
        {
            // id is the order id
             return await  _baseService.SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.DELETE,
                Url = storeUrl + "/api/orderDetailAPI/" + orderId,
                
            });
        }

      

        public async Task<T> GetAllAsync<T>()
        {
            var res = await  _baseService.SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = storeUrl + "/api/orderDetailAPI",

            });
            return res;
        }

        public async Task<T> GetAsync<T>(int OrderId, int productId)
        {
            var res = await _baseService.SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = storeUrl + "/api/orderDetailAPI/" + OrderId + "/" + productId,

            });
            return res;
        }

        public async Task<T> UpdateAsync<T>(OrderDetailUpdateDTO dto)
        {
             return await  _baseService.SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.PUT,
                Data = dto,
                Url = storeUrl + "/api/orderDetailAPI/" + dto.OrderId +"/"+ dto.ProductId ,
                
            });
        }

        public async Task<T> DeleteEachAsync<T>(int orderId, int productId)
        {
            // id is the order id
            return await _baseService.SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.DELETE,
                Url = storeUrl + "/api/orderDetailAPI/" + orderId + "/" + productId,

            });
        }
        //Url = $"{storeUrl}/api/OrderDetailAPI/{dto.OrderId}/{dto.ProductId}"
    }
}
