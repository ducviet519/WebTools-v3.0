using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppBVTA.Models
{
    public class UserModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string EmailAddress { get; set; }
        public string Role { get; set; }
        public string Permission { get; set; }
        public string Source { get; set; }
        public string DisplayName { get; set; }
        public bool Status { get; set; }
    }
}
