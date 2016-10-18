using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Insect.Domain;

namespace Insect.Tests
{
    public class TestData
    {
        public static User CreateUser()
        {
            return new User
            {
                Username = "mjashanks@hotmail.com",
                Salt = "MmmSalty",
                Id = Guid.NewGuid(),
                IsLocked = false,
                FailedLoginCount = 0,
                MobileFor2Factor = "07777388737",
                PasswordExpiryDate = DateTime.Now.AddDays(1),
                SecurityQuestion1 = "DoB",
                SecurityAnswer1 = string.Format("{0:dd/MM/yyyy}", new DateTime(1984, 04, 20)),
                SecurityQuestion2 = "Dogs Name",
                SecurityAnswer2 = "Bobby",
                UserLevel = UserLevel.Administrator
            };
        }
    }
}
