using BusinessObject.models.Dto.OrderDTO;

namespace eStore.Services.IServices
{
    public interface IOrderService
    {
        Task<T> GetAllAsync<T>();
        Task<T> GetAsync<T>(int id);
        Task<T> CreateAsync<T>(OrderCreateDTO dto);
        Task<T> UpdateAsync<T>(OrderUpdateDTO dto);
        Task<T> DeleteAsync<T>(int id);
    }
}
