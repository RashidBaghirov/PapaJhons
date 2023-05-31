﻿using System.ComponentModel.DataAnnotations;

namespace Papa_Jhons.ViewModel
{
    public class ProfileVM
    {
        public string? UserName { get; set; }

        public string? FullName { get; set; }

        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }

        [DataType(DataType.Password)]
        public string? NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare(nameof(NewPassword))]
        public string? ConfirmNewPassword { get; set; }

        [DataType(DataType.Password)]
        public string? CurrentPassword { get; set; }

    }
}
