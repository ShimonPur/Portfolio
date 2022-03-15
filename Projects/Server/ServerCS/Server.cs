using ServerCS.Interfaces;
using ServerCS.DB;

namespace ServerCS
{
    public class LogMessageEventArgs : EventArgs
    {
        public string? LogMessage { get; set; }
    }

    public sealed class Server
    {
        private static readonly Lazy<Server> instance = new(() => new Server());
        private bool _close = false;
        private bool _gui = false;
        public bool GUI
        {
            get => _gui;
            set => _gui = value;
        }

        public bool Close
        {
            get => _close;
            set => _close = value;
        }


        Communicator _communicator;
        IDatabase _database;

        private Server()
        {
            _communicator = Communicator.Instance;
            _database = SQLiteDataBase.Instance;
        }

        public static Server Instance => instance.Value;

        public void Run()
        {
            Thread t_connector = new(() => _communicator?.StartHandleRequest());

            t_connector.Start();

            Exit();

            Environment.Exit(0);
        }

        public void Log(string messageToLog)
        {
            if(_gui)
            {
                OnLog(messageToLog);
            }
            else
            {
                Console.WriteLine(messageToLog);
            }
        }

        public void Exit()
        {
            if(_gui)
            {
                while (!_close) { }
            }
            else
            {
                string? answer;

                do
                {
                    answer = Console.ReadLine();

                } while (answer != "EXIT");
            }
        }

        #region Events

        public delegate void LogMessageEventHandler(object sender, LogMessageEventArgs e);

        public event LogMessageEventHandler? LogMessage;

        private void OnLog(string messageToLog)
        {
            LogMessage?.Invoke(this, new LogMessageEventArgs() 
            { 
                LogMessage = messageToLog 
            });
        }

        #endregion

    }
}