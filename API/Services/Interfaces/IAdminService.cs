using API.Models.DTO;
using API.Models.Requests;
using API.Models.Responses;

namespace API.Services.Interfaces
{
    public interface IAdminService
    {
        public ResponsePaginationDefault<List<ClientDTO>> GetAllAdmins(UrlQuery query);
        public ResponseDefault<CompleteAdminDTO> GetAdmin(int Id);
        public ResponseDefault<ClientDTO> PostAdmin(PostUserDTO adminDTO);
        public ResponseDefault<ClientDTO> PatchAdmin(int Id, PostUserDTO adminDTO);
        public ResponseDefault<ClientDTO> DeleteAdmin(int Id);
    }
}
