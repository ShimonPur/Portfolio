using ServerCS.Exceptions;
using ServerCS.Interfaces;
using ServerCS.Managers;
using ServerCS.Requests;
using ServerCS.Responses;

namespace ServerCS.Handlers
{
    internal class MenuRequestHandler : IRequestHandler
    {
        private readonly RequestHandlerFactory _requestHandlerFactory;
        private readonly IDatabase _database;
        private readonly LoggedUser _loggedUser;

        public MenuRequestHandler(LoggedUser loggedUser, RequestHandlerFactory requestHandlerFactory, IDatabase database)
        {
            _loggedUser = loggedUser;
            _requestHandlerFactory = requestHandlerFactory;
            _database = database; 
        }

        public RequestResult handleRequest(RequestInfo requestInfo)
        {
            if (!isRequestRelevent(requestInfo))
                // if we reach here - somthing went wrong => throw exception
                throw new ServerException("The request is unrelevent for the current state"); 

            return requestInfo._id switch
            {
                (int?)Code.LOGOUT => Logout(requestInfo),
                _ => throw new NotImplementedException()
            };
        }
        public bool isRequestRelevent(RequestInfo requestInfo)
        {
            return requestInfo._id == Code.LOGOUT;
        }

        private RequestResult Logout(RequestInfo requestInfo)
        {
            if (_requestHandlerFactory is not null)
            {
                return _requestHandlerFactory.CreateLoginRequestHandler().handleRequest(requestInfo);
            }

            return new RequestResult();
        }
    }
}
