using Insect.Domain;
using Insect.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insect.UserCommands
{
    // Manage user is an unclear requirement.....so ive split it out
    
    public class CreateUser : AuthorizedCommandBase<User, bool>
    {
        private IEmployeeStore _employeeStore;
        public CreateUser(IEmployeeStore employeeStore)
        {
            _employeeStore = employeeStore;
        }

        protected override bool Run(User req)
        {
            throw new NotImplementedException();
        }

        protected override bool IsAllowed(User sessionUser, User req)
        {
            return _employeeStore.IsAdmin(sessionUser);
        }
    }

    public class EditUser : AuthorizedCommandBase<User, bool>
    {
        private IEmployeeStore _employeeStore;
        public EditUser(IEmployeeStore employeeStore)
        {
            _employeeStore = employeeStore;
        }

        protected override bool Run(User req)
        {
            throw new NotImplementedException();
        }

        protected override bool IsAllowed(User sessionUser, User req)
        {
            return _employeeStore.IsAdmin(sessionUser);
        }
    }
     
}
