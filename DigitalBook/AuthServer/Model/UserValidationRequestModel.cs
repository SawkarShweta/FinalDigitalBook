using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using UserService.Controllers;
using UserService.Data;
using UserService.Models;

namespace AuthServer.Model
{
    public class UserValidationRequestModel
    {
        private readonly DigitalBookContext _context=new DigitalBookContext();
       
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        //public bool IsValidUserInformation(string username,string password)
        //{
        //    if (_context.UserMasters == null)
        //    {
        //        return false;
        //    }
        //    var encryptedPassword = EncryptionDecryptionController.Encrypt(password == null ? "" : password);
        //    var user = _context.UserMasters.Where(x=>x.UserName==username && x.UserPassword== encryptedPassword).Select(x=>x.UserId).SingleOrDefault();

        //    if (user > 0)
        //        return true;

        //    return false;
        //}

        public UserMaster IsValidUserInformation(string username, string password)
        {
            UserMaster user1 = null;
            if (_context.UserMasters == null)
            {
                return user1;
            }
            var encryptedPassword = EncryptionDecryptionController.Encrypt(password == null ? "" : password);
            user1 = _context.UserMasters.Where(x => x.UserName == username && x.UserPassword == encryptedPassword).SingleOrDefault();

            return user1;
        }

        //public User ValidateCredentials(string UserName, string Password)
        //{
           
        //    user1 = (from x in _context.Users where x.UserName == UserName && x.UserPassword == EncryptionDecryption.EncodePasswordToBase64(Password) select x).SingleOrDefault();


        //    return user1;
        //}
    }
}
