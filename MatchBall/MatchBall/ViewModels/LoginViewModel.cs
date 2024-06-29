﻿using System.ComponentModel.DataAnnotations;

namespace MatchBall.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        public string ErrorText { get; set; }
    }
}
