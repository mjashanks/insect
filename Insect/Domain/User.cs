using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insect.Domain
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Username { get; set; }
        public string Salt { get; set; }
        public string MobileFor2Factor { get; set; }
        public string SecurityQuestion1 { get; set; }
        public string SecurityAnswer1 { get; set; }
        public string SecurityQuestion2 { get; set; }
        public string SecurityAnswer2 { get; set; }
        public int FailedLoginCount { get; set; }
        public DateTime PasswordExpiryDate { get; set; }
        public bool IsAdministrator { get; set; }
        public bool IsLocked { get; set; }
    }
}
