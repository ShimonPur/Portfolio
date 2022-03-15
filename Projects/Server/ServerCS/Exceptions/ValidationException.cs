namespace ServerCS.Exceptions
{
    internal class ValidationException : Exception
    {
        public ValidationException(string message) : base(message) { }
    }
}
