using API.Models.DTO;
using API.Models.Requests;
using API.Models.Responses;

namespace API.Services.Interfaces
{
    public interface IClientService
    {
        public ResponsePaginationDefault<List<ClientDTO>> GetAllClients(UrlQuery query);
        public ResponseDefault<CompleteClientDTO> GetClient(int Id);
        public ResponseDefault<ClientDTO> PostClient(PostClientDTO clientDTO);
        public ResponseDefault<ClientDTO> PatchClient(int Id, PostClientDTO clientDTO);
        public ResponseDefault<ClientDTO> DeleteClient(int Id);
    }
}
