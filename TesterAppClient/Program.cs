using MqttClient.ClientExtension;
using MqttDataStructures;

namespace ClientTesterApp;

internal static class ClientTester
{
    private const string Host = "192.168.137.236";

    // private const string Host = "127.0.0.1";
    private const string BartId = "Bart";
    private const string TomId = "Tom";
    private const string JohnId = "John";

    private const int Port = 8068;

    // private const int Port = 1883;
    private const string Username = "ImUser";
    private const string Password = "ImUser";


    private static void Main(string[] args)
    {
        ClientFactory factory = new ClientFactory();

        IMqttClient client = factory.CreateClient();
        
        while (true)
        {
            Console.Out.WriteLine("tests started");
            client.Connect(factory.ConnectOptionsBuilder()
                .WithServer(Host, Port)
                .WithClientId(BartId)
                .WithCleanSession(true)
                .Build()
            );

            Thread.Sleep(1000);

            client.Subscribe(factory.SubscribeOptionsBuilder()
                .WithDup(true)
                .WithMessageIdentifier(1)
                .WithTopicFilters(new List<Tuple<string, QoS>>()
                {
                    new("temp", QoS.FireAndForget)
                })
                .Build()
            );

            Thread.Sleep(1000);

            client.Unsubscribe(factory.UnsubscribeOptionsBuilder()
                .WithDup(false)
                .WithQos(QoS.FireAndForget)
                .WithMessageIdentifier(1)
                .WithTopicsFilters(new List<string>()
                {
                    "temp"  
                })
                .Build());

            Thread.Sleep(1000);
        
            client.Subscribe(factory.SubscribeOptionsBuilder()
                .WithDup(true)
                .WithMessageIdentifier(1)
                .WithTopicFilters(new List<Tuple<string, QoS>>()
                {
                    new("temp", QoS.FireAndForget)
                })
                .Build()
            );

            Thread.Sleep(1000);
            
            client.Publish(factory.PublishOptionsBuilder()
                .WithQoS(QoS.FireAndForget)
                .WithDup(false)
                .WithRetain(false)
                .WithTopicName("temp")
                .WithMessageIdentifier(1)
                .WithPayload(666.ToString())
                .Build()
            );
            
            Thread.Sleep(5000);

            client.Disconnect();
            
            Thread.Sleep(1000);
            Console.Out.WriteLine("done");
        }
    }

    public static void PrintMessageBox(string topic, string message)
    {
        Console.WriteLine(topic);
        Console.WriteLine(message);
    }
}