using Insect.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insect.Stores
{
    public interface IEmployeeStore
    {
        bool IAmManagerOf(User me, int employeeId);
        bool IsHr(int employeeId);
        bool IAmHr(User me);
        bool IsMe(User me, int employeeId);
        bool IsAdmin(User me); 
    }
}
