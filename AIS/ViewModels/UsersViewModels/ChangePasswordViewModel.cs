﻿namespace AIS.ViewModels.UsersViewModels
{
    public class ChangePasswordViewModel
    {
        public string Id { get; set; }
        public string UserNickName { get; set; }
        public string Email { get; set; }
        public string NewPassword { get; set; }
        public string OldPassword { get; set; }
    }
}
