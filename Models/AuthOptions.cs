using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace chat_api.Models
{
    public class AuthOptions
    {
        public const string ISSUER = "localhost:5000"; // издатель токена
        public const string AUDIENCE = "localhost:300"; // потребитель токена
        const string KEY = "My_Chat1_ApiKeY!";   // ключ для шифрации
        public const int LIFETIME = 2880; // время жизни токена - 1 минута
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
        }
    }
}