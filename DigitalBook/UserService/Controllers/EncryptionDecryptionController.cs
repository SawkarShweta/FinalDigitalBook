using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Text;

namespace UserService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EncryptionDecryptionController : ControllerBase
    {
        public static string Encrypt(string password)
        {
            try
            {
                string encryptedData = string.Empty;
                byte[] encode = new byte[password.Length];
                encode = Encoding.UTF8.GetBytes(password);
                encryptedData = Convert.ToBase64String(encode);
                return encryptedData;
            }
            catch(Exception e)
            {
                throw new Exception("Error occurred during encryption: ",e);
            }
        }

        public static string Decrypt(string encryptpwd)
        {
            try
            {
                string decryptpwd = string.Empty;
                UTF8Encoding encodepwd = new UTF8Encoding();
                Decoder Decode = encodepwd.GetDecoder();
                byte[] todecode_byte = Convert.FromBase64String(encryptpwd);
                int charCount = Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
                char[] decoded_char = new char[charCount];
                Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
                decryptpwd = new String(decoded_char);
                return decryptpwd;
            }
            catch(Exception e)
            {
                throw new Exception("Error occurred during decryption: ",e);
            }
        }
    }
}
