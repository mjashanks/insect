using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insect.Authentication
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

        public static bool IsValid(UserRegistrationDetails details)
        {
            if (details == null) return false;
            if (Empty(details.Username)) return false;
            if (Empty(details.Password)) return false;
            if (Empty(details.MobileFor2Factor)) return false;
            if (Empty(details.SecurityQuestion1)) return false;
            if (Empty(details.SecurityAnswer1)) return false;
            if (Empty(details.SecurityQuestion2)) return false;
            if (Empty(details.SecurityAnswer2)) return false;
            return true;
        }

        private static bool Empty(string val)
        {
            return string.IsNullOrWhiteSpace(val);
        }
    }
}
