using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DisneyAPI.ViewModel.Auth.Login
{
    public class LoginResVM
    {
        public string Token { get; set; }
        public DateTime ValidTo { get; set; }
    }
}
