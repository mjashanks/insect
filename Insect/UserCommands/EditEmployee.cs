using Insect.Domain;
using Insect.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insect.UserCommands
{
    public class EditEmployee : AuthorizedCommandBase<Employee, bool>
    {
        private IEmployeeStore _employeeStore;
        public EditEmployee(IEmployeeStore employeeStore)
        {
            _employeeStore = employeeStore;
        }


        protected override bool Run(Employee req)
        {
            throw new NotImplementedException();
        }

        protected override bool IsAllowed(User sessionUser, Employee req)
        {
            return _employeeStore.IAmManagerOf(sessionUser, req.Id);
        }
    }
}
