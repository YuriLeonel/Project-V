using API.Models;

namespace API.Repositories.Interfaces
{
    public interface IClient
    {
        public List<Client> GetAllClients();
        public Client GetClient(int Id);
        public Client GetClientByEmail(string Email);
        public void PostClient(Client client);
        public void PatchClient(Client client);
        public void DeleteClient(Client client);
    }
}
