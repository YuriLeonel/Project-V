using API.Models;

namespace API.Repositories.Interfaces
{
    public interface IEmployeeRepository
    {
        public List<Client> GetAllEmployees();
        public Client GetEmployee(int Id);
        public Client GetCompleteEmployee(int Id);
        public Client GetEmployeeByEmail(string Email);
        public void PostEmployee(Client employee);
        public void PatchEmployee(Client employee);
        public void DeleteEmployee(Client employee);
    }
}
