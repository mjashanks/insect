using Insect.Authentication;
using Insect.Stores;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Insect.Tests.AuthenticationTests
{
    public abstract class AuthenticationTestBase
    {
        protected AuthenticationService _authenticationService;
        protected Mock<IAuthStore> _authStore;

        public AuthenticationTestBase()
        {
            _authStore = new Mock<IAuthStore>();
            _authenticationService = new AuthenticationService(_authStore.Object);
        }
    }
}
