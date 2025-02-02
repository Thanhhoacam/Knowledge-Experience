using BusinessObject.models.Dto.ProductDto;

namespace eStore.Services.IServices
{
    public interface IProductService
    {
        //Task<T> GetAllAsync<T>(string token);
        //Task<T> GetAsync<T>(int id, string token);
        //Task<T> CreateAsync<T>(ProductCreateDTO dto, string token);
        //Task<T> UpdateAsync<T>(ProductUpdateDTO dto, string token);
        //Task<T> DeleteAsync<T>(int id, string token);

        // no token
        Task<T> DeleteAsync<T>(int id);
        Task<T> UpdateAsync<T>(ProductUpdateDTO dto);
        Task<T> CreateAsync<T>(ProductCreateDTO dto);
        Task<T> GetAllAsync<T>();
        Task<T> GetAllAsync<T>(string search, int? pageSize, int? pageNumber);
        Task<T> GetAsync<T>(int id);



    }

}
