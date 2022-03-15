namespace ServerCS
{
    public class App
    {
        [Obsolete]
        public static void Main(string [] args) 
        {
            Server server = Server.Instance;

            try
            {
                server.Run();
            }
            catch (Exception ex)
            {
                server.Log(ex.Message);
            }
        }
    }
}
