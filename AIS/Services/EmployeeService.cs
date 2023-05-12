using AIS.ViewModels.EmployersViewModels;
using Infrastructure;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System.Net;
using AIS.ErrorManager;


namespace AIS.Services
{
    public class EmployeeService : IEmployeeService
    {
        private AisDbContext db;

        public EmployeeService(AisDbContext context)
        {
            db = context;
        }

        public async Task<Employee> GetEmployee(int id)
        {

            Employee? employee = await db.Employeers.FirstOrDefaultAsync(p => p.Id == id);
            if(employee == null) throw new AisException("Сотрудник не найден", HttpStatusCode.BadRequest);
            return employee;

        }

        public async Task CreateEmployee(CreateEmployeeViewModel model)
        {
            try 
            {
                Employee employee = new Employee()
                {
                    Name = model.Name,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Address = model.Address,
                    PhoneNumber = model.PhoneNumber,
                    Email = model.Email,
                    PartnerId = model.PartnerId
                };

                db.Employeers.Add(employee);
                await db.SaveChangesAsync();
            }
            catch
            {
                throw new AisException("Не удалось создать сотрудника", HttpStatusCode.BadRequest);
            }
        }

        public async Task EditEmployee(EditEmployeeViewModel model)
        {
            try
            {
                Employee? employee = await GetEmployee(model.Id);
                if (employee == null) throw new AisException("Сотрудник не найден", HttpStatusCode.BadRequest);

                    employee.Name = model.Name;
                    employee.FirstName = model.FirstName;
                    employee.LastName = model.LastName;
                    employee.Address = model.Address;
                    employee.PhoneNumber = model.PhoneNumber;
                    employee.Email = model.Email;
                    employee.PartnerId = model.PartnerId;
                    db.Employeers.Update(employee);
                    await db.SaveChangesAsync();


            }
            catch
            {
                throw new AisException("Не удалось отредактировать данные сотрудника", HttpStatusCode.BadRequest);
            }
        }

        public async Task DeleteEmployee(int id)
        {
            try
            {
                Employee? employee = await GetEmployee(id);
                if (employee == null) throw new AisException("Сотрудник не найден", HttpStatusCode.BadRequest);
                db.Entry(employee).State = EntityState.Deleted;
                await db.SaveChangesAsync();
            }
            catch
            {
                throw new AisException("Не удалось удалить сотрудника", HttpStatusCode.BadRequest);
            }
        }
    }
}
