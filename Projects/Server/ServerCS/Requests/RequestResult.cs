using ServerCS.Interfaces;

namespace ServerCS.Requests
{
    internal struct RequestResult
    {
        public List<byte>? _buffer;
        public IRequestHandler? _newHandler;
        public RequestResult(List<byte> buffer, IRequestHandler newHandler)
        {
            _buffer = buffer;
            _newHandler = newHandler;
        }

        public RequestResult()
        {
            _buffer = new List<byte>();
            _newHandler = null;
        }
    }
}
