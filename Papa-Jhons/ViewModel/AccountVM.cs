﻿using Papa_Jhons.Entities;
using System.ComponentModel.DataAnnotations;

namespace Papa_Jhons.ViewModel
{
    public class AccountVM
    {
        public User User { get; set; }
        public string Token { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }

    }
}
