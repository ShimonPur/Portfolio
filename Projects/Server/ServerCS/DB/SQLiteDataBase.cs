using Microsoft.Data.Sqlite;
using ServerCS.Exceptions;
using ServerCS.Interfaces;

namespace ServerCS.DB
{
    public struct QueriesConstants
    {
        public static string DBUsers = "DB_Users.db";
        public static string CreateUserTable = @"
                                                    CREATE TABLE Users
                                                    (
		                                                Username TEXT NOT NULL,
		                                                Password TEXT NOT NULL,
		                                                Email TEXT NOT NULL
                                                    );
                                                ";
    }

    internal class SQLiteDataBase : IDatabase
    {
        private static Lazy<SQLiteDataBase> instance = new(() => new SQLiteDataBase());

        private SQLiteDataBase()
        {
            SQLitePCL.raw.SetProvider(new SQLitePCL.SQLite3Provider_e_sqlite3());

            if (!File.Exists(QueriesConstants.DBUsers)) // only if its new created DataBase
                CreateTables();
        }

        public static SQLiteDataBase Instance
        {
            get
            {
                return instance.Value;
            }
        }

        public static bool CreateTables()
        {
            try
            {
                using (var connection = new SqliteConnection($"Data Source={QueriesConstants.DBUsers}"))
                {
                    connection.Open();

                    SqliteCommand command = connection.CreateCommand();

                    command.CommandText = QueriesConstants.CreateUserTable;

                    command.ExecuteNonQuery();
                }

            }
            catch (SqliteException se)
            {
                Server.Instance.Log("Problem with creating Users Table - \n" + se.Message);
                return false;
            }
            return true;
        }

        public bool AddNewUser(string username, string password, string email)
        {
            try
            {
                if (DoesUserExist(username))
                {
                    throw new LoginException("The user already exists");
                }
                else if (DoesEmailExist(email))
                {
                    throw new LoginException("The email already exists");
                }

                using (var connection = new SqliteConnection($"Data Source={QueriesConstants.DBUsers}"))
                {
                    connection.Open();

                    SqliteCommand command = connection.CreateCommand();

                    command.CommandText =
                        @"
                            INSERT INTO Users (Username, Password, Email) 
                            Values ($username, $password, $email)
                        ;";

                    command.Parameters.AddRange(
                        new List<SqliteParameter>
                        {
                            new SqliteParameter("$username", username),
                            new SqliteParameter("$password", password),
                            new SqliteParameter("$email", email)
                        });

                    command.ExecuteNonQuery();
                }
            }
            catch(SqliteException se)
            {
                Server.Instance.Log(se.Message);
                return false;
            }

            return true;
        }

        public bool DoesPasswordMatch(string username, string password)
        {
            string? name = "", pass = "";

            if(!DoesUserExist(username))
            {
                throw new LoginException("Username/password are wrong, Try again");
            }

            try
            {
                using (var connection = new SqliteConnection($"Data Source={QueriesConstants.DBUsers}"))
                {
                    connection.Open();

                    SqliteCommand command = connection.CreateCommand();

                    command.CommandText = 
                        @"
                            SELECT Username, 
                            Password FROM Users 
                            WHERE Username = $username and Password = $password
                        ;";

                    command.Parameters.AddRange(
                        new List<SqliteParameter>
                        {
                            new SqliteParameter("$username", username),
                            new SqliteParameter("$password", password)
                        });

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            name = reader.GetString(0);
                            pass = reader.GetString(1);
                        }
                    }
                }
            }
            catch(SqliteException se)
            {
                Server.Instance.Log("Problem with matching name and password - \n" + se.Message);
            }

            return !string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(pass);// if not null - true, otherwise false
        }

        public bool DoesEmailExist(string email)
        {
            string e_mail = "";

            try
            {
                using (var connection = new SqliteConnection($"Data Source={QueriesConstants.DBUsers}"))
                {
                    connection.Open();

                    SqliteCommand command = connection.CreateCommand();

                    command.CommandText =
                    @"
                        SELECT Email
                        FROM Users 
                        WHERE Email = $email
                    ;";

                    command.Parameters.AddWithValue("$email", email);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            e_mail = reader.GetString(0);
                        }
                    }
                }
            }
            catch (SqliteException se)
            {
                Server.Instance.Log("Problem with creating Users Table - \n" + se.Message);
            }

            return !string.IsNullOrEmpty(e_mail); // if not null - true, otherwise false
        }

        public bool DoesUserExist(string username)
        {
            string? name = "";

            try
            {
                using (var connection = new SqliteConnection($"Data Source={QueriesConstants.DBUsers}"))
                {
                    connection.Open();

                    SqliteCommand command = connection.CreateCommand();

                    command.CommandText = 
                    @"
                        SELECT Username 
                        FROM Users 
                        WHERE Username = $username
                    ;";

                    command.Parameters.AddWithValue("$username", username);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            name = reader.GetString(0);
                        }
                    }
                }
            }
            catch (SqliteException se)
            {
                Server.Instance.Log("Problem with creating Users Table - \n" + se.Message);
            }

            return !string.IsNullOrEmpty(name); // if not null - true, otherwise false
        }

        public string GetMailOfUser(string username)
        {
            string e_mail = "";

            try
            {
                using (var connection = new SqliteConnection($"Data Source={QueriesConstants.DBUsers}"))
                {
                    connection.Open();

                    SqliteCommand command = connection.CreateCommand();

                    command.CommandText =
                    @"
                        SELECT Email
                        FROM Users 
                        WHERE Username = $username
                    ;";

                    command.Parameters.AddWithValue("$username", username);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            e_mail = reader.GetString(0);
                        }
                    }
                }
            }
            catch (SqliteException se)
            {
                Server.Instance.Log("Problem with creating Users Table - \n" + se.Message);
            }

            return string.IsNullOrEmpty(e_mail) ? string.Empty : e_mail; // if not null - true, otherwise false
        }
    }
}
