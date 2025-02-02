using BusinessObject.models.Dto.MemberDTO;

namespace eStore.Services.IServices
{
    public interface IMemberService
    {
        Task<T> GetAllAsync<T>();
        Task<T> GetAsync<T>(int id);
        Task<T> CreateAsync<T>(MemberCreateDTO dto );
        Task<T> UpdateAsync<T>(MemberUpdateDTO dto);
        Task<T> DeleteAsync<T>(int id );
    }
}
