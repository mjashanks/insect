using Insect.Domain;
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
        protected override bool Run(PermissionChange req)
        {
            throw new NotImplementedException();
        }

        protected override bool IsAllowed(User sessionUser, PermissionChange req)
        {
            return sessionUser.IsAdministrator;
        }
    }
}
