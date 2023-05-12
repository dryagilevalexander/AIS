using AIS.ErrorManager;
using AIS.Services;
using DocumentFormat.OpenXml.Office2010.Excel;
using Infrastructure.Models;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace AIS.ViewModels.EmployersViewModels
{
    public class EditEmployeeViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Не указано имя сотрудника")]
        public string Name { get; set; } = null!;
        [Required(ErrorMessage = "Не указано отчество сотрудника")]
        public string FirstName { get; set; } = null!;
        [Required(ErrorMessage = "Не указана фамилия сотрудника")]
        public string LastName { get; set; } = null!;
        [Required(ErrorMessage = "Не указан адрес сотрудника")]
        public string Address { get; set; } = null!;
        [Required(ErrorMessage = "Не указан телефонный номер сотрудника")]
        public string PhoneNumber { get; set; } = null!;
        [Required(ErrorMessage = "Не указан email сотрудника")]
        public string Email { get; set; } = null!;
        public int PartnerId { get; set; }

        public async Task Fill(int id, IEmployeeService _employeeService)
        {
            Employee? employee = await _employeeService.GetEmployee(id);
            if (employee == null) throw new AisException("Сотрудник не найден", HttpStatusCode.BadRequest);
            Id = employee.Id;
            Name = employee.Name;
            FirstName = employee.FirstName;
            LastName = employee.LastName;
            Address = employee.Address;
            PhoneNumber = employee.PhoneNumber;
            Email = employee.Email;
            PartnerId = employee.PartnerId.Value;
        }

    }
}

