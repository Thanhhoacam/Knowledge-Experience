using BusinessObject.models.Dto.OrderDetailDTO;

namespace eStore.Services.IServices
{
    public interface IOrderDetailService
    {
        Task<T> GetAllAsync<T>();
        Task<T> GetAsync<T>(int OrderId, int productId);
        Task<T> CreateAsync<T>(OrderDetailCreateDTO dto);
        Task<T> UpdateAsync<T>(OrderDetailUpdateDTO dto);
        Task<T> DeleteAsync<T>(int orderId);
        Task<T> DeleteEachAsync<T>(int orderId, int productId);
    }
}
