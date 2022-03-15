namespace ServerCS.Requests
{
    internal class LoginRequest
    {
        public string? _username;
        public string? _password;

        public LoginRequest(string username, string password)
        {
            _username = username;
            _password = password;
        }

        public LoginRequest()
        {
            _username = null;
            _password = null;
        }
    }

    internal class SignupRequest
    {
        public string? _username;
        public string? _password;
        public string? _email;

        public SignupRequest(string username, string password, string email)
        {
            _username = username;
            _password = password;
            _email = email;
        }

        public SignupRequest()
        {
            _username = null;
            _password = null;
            _email = null;
        }
    }
}
