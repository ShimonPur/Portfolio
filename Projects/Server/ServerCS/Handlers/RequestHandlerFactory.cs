using ServerCS.DB;
using ServerCS.Interfaces;
using ServerCS.Managers;

namespace ServerCS.Handlers
{
    enum ResponseCode
    {
        LOGIN_OK = 20,
        SIGNUP_OK = 30,
        LOGOUT_OK = 40,
        GET_ROOMS_OK = 50,
        GET_PLAYERS_OK = 60,
        JOIN_ROOM_OK = 70,
        CREATE_ROOM_OK = 80
    }

    internal class RequestHandlerFactory
    {
        private static Lazy<RequestHandlerFactory> instance = new(() => new RequestHandlerFactory());
        private readonly LoginManager _loginManager;
        private readonly IDatabase _database;
        private RequestHandlerFactory()
        {
            _loginManager = LoginManager.Instance;
            _database = SQLiteDataBase.Instance;
        }

        public static RequestHandlerFactory Instance => instance.Value;

        public LoginManager LoginManager => _loginManager; 

        public IDatabase Database => _database;
        public LoginRequestHandler CreateLoginRequestHandler()
        {
            return new LoginRequestHandler(_loginManager, this);
        }

        public MenuRequestHandler CreateMenuRequestHandlerr(LoggedUser user)
        {
            return new MenuRequestHandler(user, this, _database);
        }
    }
}
