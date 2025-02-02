using BusinessObject.models.Dto.LoginDTO;

namespace eStore.Services.IServices
{
    public interface ITokenProvider
    {
        void SetToken(LoginResponseDTO tokenDTO);
        LoginResponseDTO? GetToken();
        void ClearToken();
    }
}
