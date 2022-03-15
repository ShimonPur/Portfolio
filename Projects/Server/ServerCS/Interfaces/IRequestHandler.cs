using ServerCS.Requests;

namespace ServerCS.Interfaces
{
    internal interface IRequestHandler
    {
        // check if the incoming request is relavent by 
        // its code.
        bool isRequestRelevent(RequestInfo requestInfo); 

        // handle the incoming request by directing it to the fitting 
        // private handler
	    RequestResult handleRequest(RequestInfo requestInfo);  
    }
}
