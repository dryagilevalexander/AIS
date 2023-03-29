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

        public async Task<bool> CreateEmployee(Employee employee)
        {
            try 
            {
                db.Employeers.Add(employee);
                await db.SaveChangesAsync();
                return true;
            }
            catch
            {
            return false;
            }
        }

        public async Task<bool> EditEmployee(Employee employee)
        {
            try
            {
                db.Employeers.Update(employee);
                await db.SaveChangesAsync();
                return true;
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
