using AIS.ViewModels;
using Core;
using Microsoft.EntityFrameworkCore;


namespace AIS.Services
{
    public class EmployeeService : IEmployeeService
    {
        private CoreContext db;

        public EmployeeService(CoreContext context)
        {
            db = context;
        }

        public async Task<IEnumerable<Employee>> GetEmployeers()
        {
            try
            {
                IEnumerable<Employee>? employeers = await db.Employeers.Include(u => u.Partner).ToListAsync();
                return employeers;
            }
            catch
            {
                return null;
            }
        }

        public async Task<Employee?> GetEmployee(int id)
        {
            try 
            { 
            Employee? employee = await db.Employeers.FirstOrDefaultAsync(p => p.Id == id);
            return employee;
            }
            catch
            {
            return null;
            }
        }

        public async Task<bool> CreateEmployee(EmployeeViewModel evm)
        {
            try 
            {
                Employee employee = new Employee()
                {
                    Name = evm.Name,
                    FirstName = evm.FirstName,
                    LastName = evm.LastName,
                    Address = evm.Address,
                    PhoneNumber = evm.PhoneNumber,
                    Email = evm.Email,
                    PartnerId = evm.PartnerId
                };

                db.Employeers.Add(employee);
                await db.SaveChangesAsync();
                return true;
            }
            catch
            {
            return false;
            }
        }

        public async Task<bool> EditEmployee(EmployeeViewModel evm)
        {
            try
            {
                Employee? employee = await GetEmployee(evm.Id);
                if (employee != null)
                {
                    employee.Name = evm.Name;
                    employee.FirstName = evm.FirstName;
                    employee.LastName = evm.LastName;
                    employee.Address = evm.Address;
                    employee.PhoneNumber = evm.PhoneNumber;
                    employee.Email = evm.Email;
                    employee.PartnerId = evm.PartnerId;
                    db.Employeers.Update(employee);
                    await db.SaveChangesAsync();
                    return true;
                }
                else return false;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteEmployee(int? id)
        {
            if (id != null)
            {
                Employee employee = new Employee { Id = id.Value };
                db.Entry(employee).State = EntityState.Deleted;
                await db.SaveChangesAsync();
                return true;    
            }
            return false;
        }
    }
}
