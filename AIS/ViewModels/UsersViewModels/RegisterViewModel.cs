﻿using System.ComponentModel.DataAnnotations;

namespace AIS.ViewModels.UsersViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Имя")]
        public string UserNickName { get; set; }

        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтвердить пароль")]
        public string PasswordConfirm { get; set; }
    }
}
