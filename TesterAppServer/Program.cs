using MqttServer;
using MqttServer.Utils;

namespace TesterAppServer;

internal static class ServerTester
{
    private const int Port = 1883;
    // private const string Host = "127.0.0.1";
    private const string Host = "0.0.0.0";

    private static void Main(string[] args)
    {
        Console.WriteLine("Creating Server!");

        IBroker broker = new Broker(
            Port, 
            Host, 
            new List<Account>()
            {
                new Account("rafal", new PassHash("aqq123")),
                new Account("GUEST")
            });

        Console.WriteLine("Server Created!\n#########################################################");

        broker.Start();
    }
}