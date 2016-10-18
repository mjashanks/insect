using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Insect.Domain;
using Insect.Authentication;

namespace Insect.Tests
{
    public class TestData
    {
        public static User User()
        {
            return new User
            {
                Username = "mjashanks@hotmail.com",
                Salt = "MmmSalty",
                Id = 99,
                IsLocked = false,
                FailedLoginCount = 0,
                MobileFor2Factor = "07777388737",
                PasswordExpiryDate = DateTime.Now.AddDays(1),
                SecurityQuestion1 = "DoB",
                SecurityAnswer1 = string.Format("{0:dd/MM/yyyy}", new DateTime(1984, 04, 20)),
                SecurityQuestion2 = "Dogs Name",
                SecurityAnswer2 = "Bobby",
                IsAdministrator = true
            };
        }

        public static UserRegistrationDetails RegDetails()
        {
            return new UserRegistrationDetails
            {
                Username = "mjashanks@hotmail.com",
                Password = "CorrectHorseBatteryStaple",
                SecurityQuestion1 = "DOB",
                SecurityAnswer1 = string.Format("{0:dd/MM/yyyy}", new DateTime(1984,04,20)),
                SecurityQuestion2 = "Dogs Name",
                SecurityAnswer2 = "Bobby",
                MobileFor2Factor = "077728837"
            };
        }
    }
}
