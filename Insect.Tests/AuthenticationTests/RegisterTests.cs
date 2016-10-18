using Insect.Authentication;
using Insect.Domain;
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

    [TestClass]
    public class RegisterTests : AuthenticationTestBase
    {
        [TestMethod]
        public void Register___when_valid___should_update_user_with_matching_details()
        {
            var details = TestData.RegDetails();
            var existingUser = TestData.User();

            _authStore.Setup(a => a.GetUserByName(details.Username))
                      .Returns(existingUser);
            
            _authStore.Setup(a => 
                a.SaveUser(It.Is<User>(u =>
                    u.FailedLoginCount == 0
                    && u.MobileFor2Factor == details.MobileFor2Factor
                    && u.SecurityAnswer1 == details.SecurityAnswer1
                    && u.SecurityAnswer2 == details.SecurityAnswer2
                    && u.SecurityQuestion1 == details.SecurityQuestion1
                    && u.SecurityQuestion2 == details.SecurityQuestion2
                )))
                .Verifiable();
            
            var result = _authenticationService.Register(details);
                       
            Assert.IsTrue(result);
            _authStore.Verify();
        }

        [TestMethod]
        public void Register___when_valid___should_store_hashed_password_and_generate_salt()
        {
            var details = TestData.RegDetails();
            var existingUser = TestData.User();
            existingUser.Salt = "";

            _authStore.Setup(a => a.GetUserByName(details.Username))
                      .Returns(existingUser);

            _authStore.Setup(a =>
                a.SaveUser(It.Is<User>(u => u.Salt.Length > 0)))
                .Verifiable();

            // ned to get this lazily, as salt is changed by service...
            Func<byte[]> expectedHash = () => PasswordHasher.GenerateSaltedHash(details.Password, existingUser.Salt);
            
            _authStore.Setup(a => 
                a.SavePasswordHash(existingUser.Id, It.Is<byte[]>(h => PasswordHasher.CompareByteArrays(expectedHash(), h)))
                ).Verifiable();
                      

            var result = _authenticationService.Register(details);

            Assert.IsTrue(result);
            _authStore.Verify();
        }

        [TestMethod]
        public void Register___when_username_not_exists___should_return_false()
        {
            var details = TestData.RegDetails();
            _authStore.Setup(a => a.GetUserByName(details.Username))
                      .Returns<User>(null);

            var result = _authenticationService.Register(details);
            Assert.IsFalse(result);
        }
    }
}
