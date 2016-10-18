using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insect.Authentication
{
    public interface IAuthenticationService
    {
        /// <summary>
        /// Login. creates session and returns a sessionId
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>SessionId</returns>
        Guid? Login(string username, string password); //returns sessionid

        /// <summary>
        /// Register a new user
        /// </summary>
        /// <param name="details">Fields required to create a user record. All fields mandatory</param>
        /// <returns>Success or not</returns>
        bool Register(UserRegistrationDetails details); 

        /// <summary>
        /// Verifies security questions, and sends 2 factor verification code to mobile. Creates session, and saves this code to the session
        /// </summary>
        /// <param name="username"></param>
        /// <param name="securityAnswer1"></param>
        /// <param name="securityAnswer2"></param>
        /// <returns>SessionId</returns>
        Guid ForgotPassword(string username, string securityAnswer1, string securityAnswer2);

        /// <summary>
        /// returns security questions for user - returns random if user does nto exist.
        /// </summary>
        /// <param name="username"></param>
        /// <returns>ordered security questions</returns>
        Tuple<string,string> GetSecurityQuestions(string username);

        /// <summary>
        /// changes the password of supplied session
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="oldPassword"></param>
        /// <param name="newPassword"></param>
        /// <returns>successfullness</returns>
        bool ChangePassword(Guid sessionId, string oldPassword, string newPassword);

        /// <summary>
        /// Verifies 2factor code, and sets new password on success
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="verificationCode"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        bool TwoFactorVerify(Guid sessionId, string verificationCode, string newPassword);

        /// <summary>
        /// Admin only - clears all locks on account
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="userName"></param>
        /// <returns>successfullness</returns>
        bool ResetAccount(Guid sessionId, string userName);
    }
}
