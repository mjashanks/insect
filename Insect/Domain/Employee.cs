using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insect.Domain
{
    public class Employee
    {
        public int Id { get; set; }
        public int? ManagerEmployeeId { get; set; }
        public int UserId { get; set; }
    }
}
