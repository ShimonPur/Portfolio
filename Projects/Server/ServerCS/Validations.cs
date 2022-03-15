
using System.Text.RegularExpressions;

namespace ServerCS
{
    internal class Validations
    {
        public static bool IsValidEmail(string email)
        {
            return new Regex("(\\w+)(\\.|_)?(\\w*)@(\\w+)(\\.(\\w+))+").IsMatch(email);
        }

        public static bool IsValidPhoneNumber(string phoneNumber)
        {
            return new Regex("0[0-9][0-9]?-\\d+").IsMatch(phoneNumber);
        }
        public static bool IsValidPassword(string password)
        {
            return new Regex("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{8,}$").IsMatch(password);
        }
    }
}
