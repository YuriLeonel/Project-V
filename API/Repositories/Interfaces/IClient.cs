using API.Models.DTO;

namespace API.Repositories.Interfaces
{
    public interface IClient
    {
        public List<ClientDTO> GetAllClients();
        public ClientDTO GetClient(int Id);
        public ClientDTO PostClient(ClientDTO clientDTO);
        public ClientDTO PutClient(ClientDTO clientDTO);
        public void DeleteClient(int Id);
        public ClientTokenDTO Login (ClientLoginDTO loginDTO);
    }
}
