using System.ComponentModel.DataAnnotations;

namespace AIS.ViewModels.EmployersViewModels
{
    public class CreateEmployeeViewModel
    {
        public int PartnerId { get; set; }
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
    }


}

