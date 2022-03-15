namespace ServerCS.Interfaces
{
    internal interface IDatabase
    {
        public bool DoesUserExist(string username);
	    public bool DoesEmailExist(string email);
	    public bool DoesPasswordMatch(string username, string password);
	    public bool AddNewUser(string username, string password, string email);
    }
}
