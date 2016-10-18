using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insect.Domain
{
    public enum AuthAuditType
    {
        LoginSuccess,
        LoginFailed_Username,
        LoginFailed_Password,
        Locked_FailedPasswords,
        Locked_FailedQuestions
    }

    internal class AuthAudit
    {
        [Key]
        public int id { get; set; }
        public DateTime Timestamp { get; set; }
        public AuthAuditType Type { get; set; }
        public Guid? UserId { get; set; } 
    }
}
