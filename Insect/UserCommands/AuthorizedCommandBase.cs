using Insect.Domain;
using Insect.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insect.UserCommands
{
    public abstract class AuthorizedCommandBase<TRequest, TResponse>
    {
        public TResponse Run(Guid sessionId, TRequest req)
        {
            var user = SessionUser(sessionId);

            if (!IsAllowed(user, req))
                throw new Exception("Not Authorized");

            return Run(req);
        }

        private User SessionUser(Guid sessid)
        {
            throw new NotImplementedException();
        }

        protected abstract TResponse Run(TRequest req);

        protected abstract bool IsAllowed(User sessionUser, TRequest req);
    }
}
