using ServerCS.Interfaces;
using ServerCS.Managers;
using ServerCS.Requests;
using ServerCS.Responses;
using ServerCS.Exceptions;
using ServerCS.Json;

namespace ServerCS.Handlers
{
    internal class LoginRequestHandler : IRequestHandler
    {
        private LoginManager? _loginManager;
        private RequestHandlerFactory? _requestHandlerFactory;

        public LoginRequestHandler(LoginManager loginManager, RequestHandlerFactory requestHandlerFactory)
        {
            _loginManager = loginManager;
            _requestHandlerFactory = requestHandlerFactory;
        }
        public RequestResult handleRequest(RequestInfo requestInfo)
        {
            if (!isRequestRelevent(requestInfo)) 
                throw new ServerException("The request is unrelevent for the current state"); // if we reach here - somthing went wrong => throw exception

            return requestInfo._id switch
            {
                (int?)Code.LOGIN => Login(requestInfo),
                (int?)Code.SIGNUP => Signup(requestInfo),
                (int?)Code.LOGOUT => Logout(requestInfo),
                _ => throw new Exception("Only god knows!"),
            };
        }

        public bool isRequestRelevent(RequestInfo requestInfo)
        {
            return requestInfo._id == Code.LOGIN || requestInfo._id == Code.SIGNUP || requestInfo._id == Code.LOGOUT;
        }

        private RequestResult Login(RequestInfo requestInfo)
        {
            LoginRespones? loginRespones = new();
            string? username = "";

            try
            {
                if (requestInfo._buffer is null) 
                    throw new ArgumentNullException("Response buffer is null");

                LoginRequest? loginRequest = JsonRequestPacketDeserializer.DeserializeLoginRequest(requestInfo._buffer);

                if(loginRequest is null) 
                    throw new ArgumentNullException("Response buffer is null");

                _loginManager?.Login(loginRequest._username, loginRequest._password);

                loginRespones.status = (uint)ResponseCode.LOGIN_OK;
                loginRespones.errorMsg = "";

                username = loginRequest._username;
            }
            catch (LoginException le)
            {
                loginRespones.status = Code.SERVER_ERROR;
                loginRespones.errorMsg = le.Message;
            }
            catch(ArgumentNullException ane)
            {
                Console.WriteLine(ane.Message);
                Server.Instance.Log(ane.Message);

                loginRespones.status = Code.SERVER_ERROR;
                loginRespones.errorMsg = "Server error";
            }

            if(loginRespones.status == (uint)ResponseCode.LOGIN_OK && _requestHandlerFactory is not null && username is not null)
            {
                return new RequestResult(JsonResponsePacketSerializer.SerializeResponse(loginRespones), _requestHandlerFactory.CreateMenuRequestHandlerr(new LoggedUser(username))); // TODO create menuRequestHandler
            }

            return new RequestResult(JsonResponsePacketSerializer.SerializeResponse(loginRespones), this);
        }

        private RequestResult Logout(RequestInfo requestInfo)
        {
            try
            {
                if (requestInfo._buffer is null)
                    throw new ArgumentNullException("Response buffer is null");

                LoginRequest? logoutRequest = JsonRequestPacketDeserializer.DeserializeLoginRequest(requestInfo._buffer);

                if (logoutRequest is null)
                    throw new ArgumentNullException("Response buffer is null");

                _loginManager?.Logout(logoutRequest._username);
            }
            catch (ArgumentNullException ane)
            {
                Console.WriteLine(ane.Message);
                Server.Instance.Log(ane.Message);
            }

            return new RequestResult(JsonResponsePacketSerializer.SerializeResponse(new LogoutResponse((uint)ResponseCode.LOGOUT_OK, "")), this);
        }

        private RequestResult Signup(RequestInfo requestInfo)
        {
            SignupResponse? signupRespones = new();
            string? username = "";

            try
            {
                if (requestInfo._buffer is null)
                    throw new ArgumentNullException("Response buffer is null");

                SignupRequest? signupRequest = JsonRequestPacketDeserializer.DeserializeSignupRequest(requestInfo._buffer);

                if (_loginManager is null)
                    throw new ArgumentNullException("Login manager is null");

                if (signupRequest is null)
                    throw new ArgumentNullException("Signup request is null");

                _loginManager.Signup(signupRequest._username, signupRequest._password, signupRequest._email);
                _loginManager.Login(signupRequest._username, signupRequest._password);

                signupRespones.status = (uint)ResponseCode.SIGNUP_OK;
                signupRespones.errorMsg = "";

                username = signupRequest._username;
            }
            catch (LoginException le)
            {

                signupRespones.status = Code.SERVER_ERROR;
                signupRespones.errorMsg = le.Message;
            }
            catch (ValidationException ve)
            {

                signupRespones.status = Code.SERVER_ERROR;
                signupRespones.errorMsg = ve.Message;
            }
            catch (ArgumentNullException ane)
            {
                Console.WriteLine(ane.Message);
                Server.Instance.Log(ane.Message);

                signupRespones.status = Code.SERVER_ERROR;
                signupRespones.errorMsg = "Server error";
            }

            if(signupRespones.status == (uint)ResponseCode.SIGNUP_OK && _requestHandlerFactory is not null && username is not null)
            {
                return new RequestResult(JsonResponsePacketSerializer.SerializeResponse(signupRespones), _requestHandlerFactory.CreateMenuRequestHandlerr(new LoggedUser(username))); //create new menu request handler
            }

            return new RequestResult(JsonResponsePacketSerializer.SerializeResponse(signupRespones), this); //stay in the same state
        }
    }
}
