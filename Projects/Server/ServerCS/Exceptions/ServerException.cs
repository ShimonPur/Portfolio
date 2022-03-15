namespace ServerCS.Exceptions
{
    internal class ServerException : Exception
    {
        public readonly bool Disconnected = false;
        public ServerException(string message) : base(message) { }
        public ServerException(string message, bool disconnected) : base(message)
        { 
            Disconnected = disconnected;
        }
    }
}
