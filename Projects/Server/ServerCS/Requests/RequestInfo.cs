namespace ServerCS.Requests
{
    internal struct RequestInfo
    {
        public int? _id;
        public DateTime? _recivalTime;
        public List<byte>? _buffer;


        public RequestInfo(int id, DateTime recivalTime, List<byte> buffer)
        {
            _id = id;
            _recivalTime = recivalTime;
            _buffer = buffer;
        }

        public RequestInfo()
        {
            _id = 0;
            _recivalTime = null;
            _buffer = new List<byte>();
        }
    }
}
