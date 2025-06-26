using API.Models.DTO;
using API.Models.Requests;
using API.Models.Responses;

namespace API.Services.Interfaces
{
    public interface IClientService
    {
        public ResponsePaginationDefault<List<ClientDTO>> GetAllClients(UrlQuery query);
        public ResponseDefault<ClientDTO> GetClient(int Id);
        public ResponseDefault<ClientDTO> PostClient(ClientDTO clientDTO);
        public ResponseDefault<ClientDTO> PatchClient(int Id, ClientDTO clientDTO);
        public ResponseDefault<ClientDTO> DeleteClient(int Id);
    }
}
