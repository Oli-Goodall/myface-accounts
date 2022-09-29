using System;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using MyFace.Models.Database;
using MyFace.Repositories;

namespace MyFace.Helpers
{
    public class AuthenticationHelper
    {
        private readonly IUsersRepo _users;

        public AuthenticationHelper(IUsersRepo users)
        {
            _users = users;
        }
        
        public bool IsAuthenticated(string authorization)
        {
            if(authorization==null)
            {
                return false;
            }
            string myAuthorization = authorization.Substring(6);
            myAuthorization = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(myAuthorization));
            string username = myAuthorization.Split(":")[0];
            string attemptedPassword = myAuthorization.Split(":")[1];

            User thisUser = _users.GetByUsername(username);

            byte[] thisUserSalt = thisUser.Salt;
            string attemptedHashedPassword = PasswordHelper.GenerateHash(attemptedPassword, thisUserSalt);

            if(attemptedHashedPassword!=thisUser.HashedPassword)
            {
                return false;
            }
            return true;
        }
        
    }
}