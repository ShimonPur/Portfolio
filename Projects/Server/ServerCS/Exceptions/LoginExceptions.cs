namespace ServerCS.Exceptions
{
    internal class LoginException : Exception
    {
        public LoginException(string message) : base(message) { }
    }
}
