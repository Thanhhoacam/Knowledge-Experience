using BusinessObject.models.Dto.LoginDTO;
using eStore.Services.IServices;
using Store_Utility;

namespace eStore.Services
{
    public class TokenProvider : ITokenProvider
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public TokenProvider(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }


        public void ClearToken()
        {
            _contextAccessor.HttpContext?.Response.Cookies.Delete(SD.AccessToken);
        }

        public LoginResponseDTO GetToken()
        {
            try
            {
                bool hasAccessToken = _contextAccessor.HttpContext.Request.Cookies.TryGetValue(SD.AccessToken, out string accessToken);

                LoginResponseDTO tokenDTO = new()
                {
                    AccessToken = accessToken,
                };
                return hasAccessToken ? tokenDTO : null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public void SetToken(LoginResponseDTO tokenDTO)
        {
            var cookieOptions = new CookieOptions { Expires = DateTime.UtcNow.AddDays(60) };
            _contextAccessor.HttpContext?.Response.Cookies.Append(SD.AccessToken, tokenDTO.AccessToken, cookieOptions);
        }
    }
}
