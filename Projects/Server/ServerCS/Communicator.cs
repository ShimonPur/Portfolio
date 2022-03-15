using ServerCS.Interfaces;
using System.Net;
using System.Net.Sockets;
using ServerCS.Handlers;
using ServerCS.Exceptions;
using ServerCS.Requests;
using System.Text.RegularExpressions;
using System.Text;
using ServerCS.Responses;

namespace ServerCS
{
    internal class Communicator
    {
        private static readonly Lazy<Communicator> instance = new(() => new Communicator());
        private RequestHandlerFactory _requestHandlerFactory;
        private List<Thread> _clientThreads;
        private Dictionary<Socket, IRequestHandler>? _clients;
        private Socket? _serverSocket;

        private Communicator()
        {
            _requestHandlerFactory = RequestHandlerFactory.Instance;
            _clients = new Dictionary<Socket, IRequestHandler>();
            _clientThreads = new List<Thread>();
        }

        public static Communicator Instance => instance.Value; 

        private void HandleNewClient(Socket clientSocket)
        {
            try
            {
                while(true)
                {
                    RequestResult requestResult = new();

                    List<byte> buffer = GetMessageList(clientSocket);

                    RequestInfo requestInfo = new(buffer[0] - '0', DateTime.Now, buffer);

                    if (requestInfo._id == Code.EXIT) throw new ServerException("Client disconnected");

                    if (_clients is null)
                        throw new ArgumentNullException("_client is null");

                    requestResult = _clients[clientSocket].handleRequest(requestInfo);

                    if (requestResult._newHandler != _clients[clientSocket] &&
                        requestResult._newHandler is not null)
                    {
                        _clients[clientSocket] = requestResult._newHandler;
                    }

                    if (requestResult._buffer is not null)
                        clientSocket.Send(requestResult._buffer.ToArray());

                }
            }
            catch (ServerException se)
            {
                if(se.Disconnected)
                {
                    _clients?.Remove(clientSocket);
                    clientSocket.Close();
                }

                Server.Instance.Log(se.Message);
            }
            catch(Exception ex)
            {
                Server.Instance.Log(ex.Message);
            }


            Server.Instance.Log("Client disconnected");
        }

        private void BindAndListen()
        {
            IPEndPoint localEndPoint = new(IPAddress.Loopback, 6969);

            try
            {
                if (_serverSocket is null)
                    throw new ArgumentNullException("Socket is null");

                _serverSocket.Bind(localEndPoint);
                _serverSocket.Listen();
            }
            catch (Exception e)
            {
                Server.Instance.Log("Cant bind the socket - \n" + e.Message);
            }
        }

        public void StartHandleRequest()
        {
            try
            {
                _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            }
            catch (SocketException se)
            {
                Server.Instance.Log("Cant start listening socket - \n" +  se.Message);
                Environment.Exit(0); // ServerProblemException
            }

            BindAndListen();
            
            while(true)
            {
                Server.Instance.Log("Waiting for client connection request");

                Socket? clientSocket = null;

                try
                {
                    clientSocket = _serverSocket.Accept();
                }
                catch(SocketException se)
                {
                    throw se;
                }

                Server.Instance.Log("Client accepted. Server and client can speak");

                _clients?.Add(clientSocket, _requestHandlerFactory.CreateLoginRequestHandler());

                try
                {
                    Thread thread = new(() => HandleNewClient(clientSocket));
                    thread.Start();
                    _clientThreads.Add(thread);
                }
                catch(Exception e)
                {
                    Server.Instance.Log(e.Message);
                }
            }
        }

        #region Communicator Util
        private static List<byte> GetMessageList(Socket clientSocket)
        {
            byte [] buffer = new byte[1024 + 1];
            clientSocket.Receive(buffer);

            if(!IsConnected(clientSocket))
            {
                throw new ServerException("Client disconnected", true);
            }

            string clean = Encoding.Default.GetString(buffer);
            clean = StripeUnicode(ref clean);

            Server.Instance.Log($"Buffer => {clean}");

            return new List<byte>(Encoding.ASCII.GetBytes(clean));
        }

        private static bool IsConnected(Socket socket)
        {
            try
            {
                return !(socket.Poll(1, SelectMode.SelectRead) && socket.Available == 0);
            }
            catch (SocketException) 
            { 
                return false; 
            }
        }

        private static string StripeUnicode(ref string str)
        {
           return new string((from c in str
                              where c < 127 && c > 32
                              select c).ToArray());
        }

        #endregion

    }
}
