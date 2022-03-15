using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace RecipesApp.Client
{
    public readonly struct Codes
    {
        public const string EXIT = "0";
        public const string LOGIN = "2";
        public const string SIGNUP = "3";
        public const string LOGOUT = "4";
    }

    public readonly struct Connection
    {
        public const int MaxBytesRecived = 1024;
        public const string IP_ADDR = "127.0.0.1";
        public const int PORT_ADDR = 6969;
    }

    public readonly record struct ServerMsg(string errorMsg, int status);

    public readonly record struct LoginMsg(string username, string password);

    public readonly record struct SignupMsg(string username, string email, string password);

    internal class Communicator
    {
        public static IPAddress hostAddress = IPAddress.Parse(Connection.IP_ADDR);
        public static IPEndPoint hostEndPoint = new(hostAddress, Connection.PORT_ADDR);
        public static Socket socket = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        #region Utility
        public static string SendMsgAndReceive(string msg)
        {
            string result;

            try
            {
                ASCIIEncoding asciiEnc = new();
                byte[] serverAnswer = new byte[Connection.MaxBytesRecived];
                socket.Send(asciiEnc.GetBytes(msg), 0, msg.Length, SocketFlags.None); // send to the server tha user msg

                int numberOfBytes = socket.Receive(serverAnswer, 0, Connection.MaxBytesRecived, SocketFlags.None);
                result = asciiEnc.GetString(serverAnswer, 0, numberOfBytes);
            }
            catch(SocketException)
            {
                return string.Empty;
            }

            return result;
        }
        #endregion

        #region Communication
        private static string ConstructMessage(string msgCode, string jsonMsg)
        {
            const int FILL = 4;
            return msgCode + $"{jsonMsg.Length}".PadLeft(FILL, '0') + jsonMsg;
        }
        public static ServerMsg Login(string username, string password)
        {

            if (username == "" || password == "")
            {
                return new ServerMsg
                {
                    errorMsg = "Missing Details, try Again",
                    status = -1
                };
            }

            string LoginMsgJson = JsonSerializer.Serialize(new LoginMsg
            {
                username = username,
                password = password
            });

            string clientMsgToServer = ConstructMessage(Codes.LOGIN, LoginMsgJson);

            string serverMsg = SendMsgAndReceive(clientMsgToServer);

            if(serverMsg == string.Empty)
            {
                return new ServerMsg
                {
                    errorMsg = "Server SHUTDOWN",
                    status= -1
                };
            }

            return JsonSerializer.Deserialize<ServerMsg>(serverMsg[5..]);
        }

        public static ServerMsg Logout(string username)
        {
            if (username == "")
            {
                return new ServerMsg
                {
                    errorMsg = "Username missing, try again",
                    status = -1
                };
            }

            string LogoutMsgJson = JsonSerializer.Serialize(new LoginMsg
            {
                username = username,
                password = "" // in logout the passwod is unneccesary
            });

            string clientMsgToServer = ConstructMessage(Codes.LOGOUT, LogoutMsgJson);

            string serverMsg = SendMsgAndReceive(clientMsgToServer);

            if (serverMsg == string.Empty)
            {
                return new ServerMsg
                {
                    errorMsg = "Server SHUTDOWN",
                    status= -1
                };
            }

            return JsonSerializer.Deserialize<ServerMsg>(serverMsg[5..]);
        }

        public static ServerMsg Signup(string username, string email, string password)
        {
            if (username == "" || password == "" || email == "")
            {
                return new ServerMsg
                {
                    errorMsg = "Missing Details, try Again",
                    status = -1
                };
            }

            string SignupMsgJson = JsonSerializer.Serialize(new SignupMsg
            {
                username = username,
                email = email,
                password = password
            });

            string clientMsgToServer = ConstructMessage(Codes.SIGNUP, SignupMsgJson);

            string serverMsg = SendMsgAndReceive(clientMsgToServer);

            if (serverMsg == string.Empty)
            {
                return new ServerMsg
                {
                    errorMsg = "Server SHUTDOWN",
                    status= -1
                };
            }

            return JsonSerializer.Deserialize<ServerMsg>(serverMsg[5..]);
        }
        #endregion
    }
}
