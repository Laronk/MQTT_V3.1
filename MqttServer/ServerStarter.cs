using MqttServer.Utils;

namespace MqttServer;

internal static class ServerStarter
{
    private const int Port = 1883;
    private const string Host = "127.0.0.1";
    
    private static void Main(string[] args)
    {
        Console.WriteLine("Starting Server!");

        IBroker broker = new Broker(
            Port, 
            Host, 
            new List<Account>()
            {
                new Account("AGH", new PassHash("EAIIB")), 
                new Account("GUEST")
            });
        
        Console.WriteLine("Server Started!\n#########################################################");
        
        broker.Start();
    }
}