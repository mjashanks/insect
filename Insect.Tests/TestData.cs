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
                IsAdministrator = true,
                TwoFactorCode = "9938847",
                VerificationExpiryDate = DateTime.Now.AddDays(1),
                EmailVerificationPath = "unique_email_verify_url"
            };
        }
    }
}
