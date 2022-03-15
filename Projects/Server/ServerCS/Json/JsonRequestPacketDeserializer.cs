using ServerCS.Requests;
using System.Text;
using System.Text.Json;

namespace ServerCS.Json
{
    internal class JsonRequestPacketDeserializer
    {
        public static LoginRequest? DeserializeLoginRequest(List<byte> buffer)
        {
            Dictionary<string, string> request = ExtarctJsonFromBuffer(buffer);

            return new LoginRequest(request["username"], request["password"]);
        }

        public static SignupRequest? DeserializeSignupRequest(List<byte> buffer)
        {
            Dictionary<string, string> request = ExtarctJsonFromBuffer(buffer);

            return new SignupRequest(request["username"], request["password"], request["email"]);
        }

        private static Dictionary<string, string> ExtarctJsonFromBuffer(List<byte> buffer)
        {
            string json = Encoding.Default.GetString(buffer.ToArray());
            return JsonSerializer.Deserialize<Dictionary<string, string>>(json[5..])!;
        }
    }
}
