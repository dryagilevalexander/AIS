using AIS.ViewModels.EmployersViewModels;
using Infrastructure;
using Infrastructure.Models;

namespace AIS.Services
{
    public interface IEmployeeService
    {
        Task<Employee?> GetEmployee(int id);
        Task CreateEmployee(CreateEmployeeViewModel model);
        Task EditEmployee(EditEmployeeViewModel model);
        Task DeleteEmployee(int id);
    }
}
