using API.Models;

namespace API.Repositories.Interfaces
{
    public interface IAdminRepository
    {
        public List<Client> GetAllAdmins();
        public Client GetAdmin(int Id);
        public Client GetCompleteAdmin(int Id);
        public Client GetAdminByEmail(string Email);
        public void PostAdmin(Client admin);
        public void PatchAdmin(Client admin);
        public void DeleteAdmin(Client admin);
    }
}
