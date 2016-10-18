using Insect.Domain;
using Insect.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insect.UserCommands
{
    public class PermissionChange 
    {
        public int UserId {get; set;}
        // todo ... what are 'permissions' 
    }

    public class ManagePermission : AuthorizedCommandBase<PermissionChange, bool>
    {
        private IEmployeeStore _employeeStore;
        public ManagePermission(IEmployeeStore employeeStore)
        {
            _employeeStore = employeeStore;
        }


        protected override bool Run(PermissionChange req)
        {
            throw new NotImplementedException();
        }

        protected override bool IsAllowed(User sessionUser, PermissionChange req)
        {
            return _employeeStore.IsAdmin(sessionUser);
        }
    }
}
