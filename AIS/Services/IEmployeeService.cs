using AIS.ViewModels;
using Core;

namespace AIS.Services
{
    public interface IEmployeeService
    {
        Task<IEnumerable<Employee>> GetEmployeers();
        Task<Employee?> GetEmployee(int id);
        Task<bool> CreateEmployee(Employee employee);
        Task<bool> EditEmployee(Employee employee);
        Task<bool> DeleteEmployee(int? id);
    }
}
