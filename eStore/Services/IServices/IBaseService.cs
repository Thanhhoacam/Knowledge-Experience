using eStore.Models;

namespace eStore.Services.IServices
{
    public interface IBaseService
    {
        APIResponse responseModel { get; set; }
        Task<T> SendAsync<T>(APIRequest apiRequest, bool withBearer = true); // login, register, logout no need to send bearer token
    }
}
