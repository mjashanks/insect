using Insect.Domain;
using Insect.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insect.UserCommands
{
    public class ViewEmployee : AuthorizedCommandBase<int, Employee>
    {
        private IEmployeeStore _employeeStore;
        public ViewEmployee(IEmployeeStore employeeStore)
        {
            _employeeStore = employeeStore;
        }

        protected override Employee Run(int req)
        {
            throw new NotImplementedException();
        }

        protected override bool IsAllowed(User sessionUser, int employeeId)
        {
            return 
                _employeeStore.IsMe(sessionUser, employeeId)
                || _employeeStore.IAmManagerOf(sessionUser, employeeId)
                || (_employeeStore.IAmHr(sessionUser) && !_employeeStore.IsHr(employeeId));
        }
    }
}
