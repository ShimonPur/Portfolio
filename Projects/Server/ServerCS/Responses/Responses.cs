namespace ServerCS.Responses
{
	public struct Code
	{
		public const int EXIT = 0;
		public const int SERVER_ERROR = 1;
		public const int LOGIN = 2;
		public const int SIGNUP = 3;
		public const int LOGOUT = 4;
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
