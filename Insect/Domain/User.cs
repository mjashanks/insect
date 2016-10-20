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
        public string TwoFactorCode { get; set; }
        public string EmailVerificationPath { get; set; }
        public int FailedLoginCount { get; set; }
        public DateTime PasswordExpiryDate { get; set; }
        public bool IsAdministrator { get; set; }
        public bool IsLocked { get; set; }
        public bool IsVerified { get; set; }
        public DateTime VerificationExpiryDate { get; set; }
    }
}
