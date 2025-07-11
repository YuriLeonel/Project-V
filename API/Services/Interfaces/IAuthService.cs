using API.Models.DTO;
using API.Models.Responses;

namespace API.Services.Interfaces
{
    public interface IAuthService
    {
        public ResponseDefault<ClientTokenDTO> Login(ClientLoginDTO loginDTO);
        public ResponseDefault<ClientTokenDTO> RefreshToken(string refreshToken);
    }
}
