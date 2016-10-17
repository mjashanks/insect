using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nant.Authentication
{
    public class UserRegistrationDetails
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string MobileFor2Factor { get; set; }
        public string SecurityQuestion1 { get; set; }
        public string SecurityAnswer1 { get; set; }
        public string SecurityQuestion2 { get; set; }
        public string SecurityAnswer2 { get; set; }
    }
}
