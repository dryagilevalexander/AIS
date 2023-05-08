using AIS.ViewModels.ProcessViewModels;
using Core;

namespace AIS.Services
{
    public interface IEmployeeService
    {
        Task<IEnumerable<Employee>> GetEmployeers();
        Task<Employee?> GetEmployee(int id);
        Task<bool> CreateEmployee(EmployeeViewModel evm);
        Task<bool> EditEmployee(EmployeeViewModel evm);
        Task<bool> DeleteEmployee(int? id);
    }
}
