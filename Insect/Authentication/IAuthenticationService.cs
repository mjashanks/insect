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
        /// Registration of an already created user
        /// </summary>
        /// <param name="details">Fields required to create a user record. All fields mandatory</param>
        /// <returns>Success or not</returns>
        bool Verify(string username, string password, string twofactor);

        /// <summary>
        /// Login. creates session and returns a sessionId
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>SessionId</returns>
        Guid? Login(string username, string password); //returns sessionid 

        /// <summary>
        /// Verifies security questions, and sends 2 factor verification code to mobile. Creates session, and saves this code to the session
        /// </summary>
        /// <param name="username"></param>
        /// <param name="securityAnswer1"></param>
        /// <param name="securityAnswer2"></param>
        /// <returns>SessionId</returns>
        void ForgotPassword(string username);

        /// <summary>
        /// changes the password of supplied session
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="oldPassword"></param>
        /// <param name="newPassword"></param>
        /// <returns>successfullness</returns>
        bool ChangePassword(Guid sessionId, string oldPassword, string newPassword);

    }
}
