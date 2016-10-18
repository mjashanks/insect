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
        public UserLevel NewLevel {get; set;}
    }

    public class ManagePermission : AuthorizedCommandBase<PermissionChange, bool>
    {
        protected override bool Run(PermissionChange req)
        {
            throw new NotImplementedException();
        }

        protected override UserLevel[] AllowedLevels
        {
            get { return Allow(UserLevel.Administrator); }
        }
    }
}
