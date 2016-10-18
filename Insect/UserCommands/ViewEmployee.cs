using Insect.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insect.UserCommands
{
    public class ViewEmployee : AuthorizedCommandBase<int, Employee>
    {
        protected override Employee Run(int req)
        {
            throw new NotImplementedException();
        }

        protected override UserLevel[] AllowedLevels
        {
            get { return Allow(UserLevel.Employee, UserLevel.Manager); }
        }
    }
}
