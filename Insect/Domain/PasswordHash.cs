using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insect.Domain
{
    public class PasswordHash
    {
        public Guid UserId { get; set; }
        public string Hash { get; set; }
    }
}
