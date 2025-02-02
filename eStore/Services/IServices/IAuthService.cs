using BusinessObject.models.Dto.LoginDTO;
using BusinessObject.models.Dto.RegisterDTO;

namespace eStore.Services.IServices
{
    public interface IAuthService
    {
        Task<T> LoginAsync<T>(LoginRequestDTO objToCreate);
        Task<T> RegisterAsync<T>(RegisterationRequestDTO objToCreate);
    }
}
