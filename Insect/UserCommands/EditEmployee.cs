using Insect.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insect.UserCommands
{
    public class EditEmployee : AuthorizedCommandBase<Employee, bool>
    {
        protected override bool Run(Employee req)
        {
            throw new NotImplementedException();
        }

        protected override UserLevel[] AllowedLevels
        {
            get { return Allow(UserLevel.Manager); }
        }
    }
}
