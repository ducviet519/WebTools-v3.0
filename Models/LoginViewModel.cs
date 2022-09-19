using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebTools.Models.Entity;

namespace WebTools.Models
{
    public class LoginViewModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public UserAccount userAccount { get; set; }
        public string ReturnUrl { get; set; }
    }
}
