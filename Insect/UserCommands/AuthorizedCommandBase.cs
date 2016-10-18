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
            var sessionUserLevel = SessionUserLevel(sessionId);

            if (!AllowedLevels.Contains(sessionUserLevel))
                throw new Exception("Not Authorized");

            return Run(req);
        }

        private UserLevel SessionUserLevel(Guid sessid)
        {
            throw new NotImplementedException();
        }

        protected abstract TResponse Run(TRequest req);

        protected abstract UserLevel[] AllowedLevels {get;}

        protected UserLevel[] Allow(params UserLevel[] levels)
        {
            return levels;
        }
    }
}
