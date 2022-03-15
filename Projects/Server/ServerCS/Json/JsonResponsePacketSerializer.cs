using ServerCS.Responses;
using System.Text;
using System.Text.Json;

namespace ServerCS.Json
{
    internal class JsonResponsePacketSerializer
    {
        private static readonly int FILL = 4;
        public static List<byte> SerializeResponse(ErrorRespones errorRespones)
        {
            return GenerateResponse(errorRespones, Code.SERVER_ERROR);
        }

        public static List<byte> SerializeResponse(LoginRespones loginResponse)
        {
            return GenerateResponse(loginResponse, Code.LOGIN);
        }

        public static List<byte> SerializeResponse(SignupResponse signupResponse)
        {
            return GenerateResponse(signupResponse, Code.SIGNUP);
        }

        public static List<byte> SerializeResponse(LogoutResponse logoutResponse)
        {
            return GenerateResponse(logoutResponse, Code.LOGOUT);
        }

        #region Util
        private static List<byte> GenerateResponse(Response response, uint code)
        {
            string jsonString = "";

            switch (response)
            {
                case LoginRespones:
                    jsonString = JsonSerializer.Serialize(response as LoginRespones);
                    break;
                case SignupResponse:
                    jsonString = JsonSerializer.Serialize(response as SignupResponse);
                    break;
                case LogoutResponse:
                    jsonString = JsonSerializer.Serialize(response as LogoutResponse);
                    break;
                case ErrorRespones:
                    jsonString = JsonSerializer.Serialize(response as ErrorRespones);
                    break;
            }

            string generatedResponse = $"{code}" +
                                       $"{jsonString.Length}".PadLeft(FILL, '0') +
                                          jsonString;

            return new List<byte>(new ASCIIEncoding().GetBytes(generatedResponse)); // convert the string into list of bytes
        }
        #endregion
    }
}
