using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insect.Domain
{
    public class PasswordHash
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }
        public byte[] Hash { get; set; }
    }
}
