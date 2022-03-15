using ServerCS.DB;
using ServerCS.Exceptions;
using ServerCS.Interfaces;

namespace ServerCS.Managers
{
    internal class LoginManager
    {
        private static readonly Lazy<LoginManager> instance = new(() => new LoginManager());
        readonly IDatabase database;
        private readonly List<LoggedUser>? loggedUsers = null;
        private LoginManager()
        {
            database = SQLiteDataBase.Instance;
            loggedUsers = new List<LoggedUser>();
        }

        public static LoginManager Instance
        {
            get
            {
                return instance.Value;
            }
        }

        public void Signup(string? username, string? password, string? email)
        {
            if (username is null ||
                password is null ||
                email is null)
                throw new ArgumentNullException("One or more parameters are null");

            if (!Validations.IsValidEmail(email))
                throw new ValidationException("Invalid email. email is not in the correct format. Try again");

            if (!Validations.IsValidPassword(password))
                throw new ValidationException("" +
                                              "Invalid password.\n" +
                                              "password must be 8 charcters long and must contain:\n" +
                                              "- uppercase letters\n" +
                                              "- lowercase letters\n" +
                                              "- at least 1 number\n" +
                                              "- at least 1 special symbole.");

            try
            {
                database.AddNewUser(username, password, email);
            }
            catch (LoginException le)
            {
                throw le;
            }
        }

        public void Login(string? username, string? password)
        {
            if (username is null || password is null)
                throw new ArgumentNullException("One or more prameters are null");

            LoggedUser? user = new();

            user = loggedUsers?.Find(user => user.Username == username);

            if (!database.DoesPasswordMatch(username, password))
                throw new LoginException("Username/password are wrong, Try again");

            if (user is not null)
                throw new LoginException("The user already logged in to the system");

            loggedUsers?.Add(new LoggedUser(username));
        }

        public void Logout(string? username)
        {
            LoggedUser? userToDelete = new();

            userToDelete = loggedUsers?.Find(user => user.Username == username);

            if (userToDelete is not null)
                loggedUsers?.Remove(userToDelete);
        }
    }
}
