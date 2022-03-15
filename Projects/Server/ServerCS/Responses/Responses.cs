namespace ServerCS.Responses
{
	public struct Code
	{
		public const uint EXIT = 0;
		public const uint SERVER_ERROR = 1;
		public const uint LOGIN = 2;
		public const uint SIGNUP = 3;
		public const uint LOGOUT = 4;
	};

	public class Response { }
	public class ErrorRespones : Response
	{
		public string? message;
	};

	public class LoginRespones : Response
	{
		public uint status { get; set; }
		public string? errorMsg { get; set; }
	};

	public class SignupResponse : Response
	{
		public uint status { get; set; }
		public string? errorMsg { get; set; }
	};

	public class LogoutResponse : Response
	{
        public uint status { get; set; }
		public string errorMsg { get; set; }

		public LogoutResponse(uint status, string errorMsg)
		{
			this.status = status;
			this.errorMsg = errorMsg;
		}
	};

}
