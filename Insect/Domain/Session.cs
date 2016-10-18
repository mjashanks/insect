using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insect.Domain
{
    public class Session
    {
        [Key]
        public int Id { get; set; }
        public Guid SessionId { get; set; }
        public int UserId { get; set; }
        public DateTime ExpiryDate { get; set; }
        public UserLevel Level { get; set;  } // strictly not necessary - but for convenience
    }
}
