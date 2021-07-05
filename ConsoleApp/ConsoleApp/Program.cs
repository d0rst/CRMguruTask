using ConsoleApp.console;

namespace ConsoleApp
{

    class Program
    {
        static void Main(string[] args)
        {
            ConsoleClient con = new ConsoleClient();
            con.ConsoleInputAsync().Wait();
        }
    }
}
