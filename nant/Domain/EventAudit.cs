using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nant.Domain
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
        public DateTime Timestamp { get; set; }
        public AuthAuditType Type { get; set; }
        public Guid? UserId { get; set; } 
    }
}
