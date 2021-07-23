using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WalletApp.Model.ViewModel.RequestBodyModel
{
    public class LoginViewModel
    {
       
        public string Username { get; set; }

      
        public string Password { get; set; }
    }
}
