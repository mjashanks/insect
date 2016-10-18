using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insect.Domain
{
    public class Session
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime ExpiryDate { get; set; }
        public UserLevel Level { get; set;  } // strictly not necessary - but for convenience
    }
}
