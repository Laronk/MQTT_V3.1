using MqttClient.ClientCore;
using MqttClient.ClientCore.Commands;
using MqttDataStructures;
using MqttDataStructures.Messages.Pubs;

namespace MqttClient.ClientExtension.ClientCommands;

public class ClientPublishCommand : Command, IClientCommand
{
    public IMqttClient Client { get; }
    
    public override bool Execute(Message message, bool isStart)
    {
        return (message, isStart) switch
        {
            (Publish publish, true) => SendPublish(publish),
            (Publish publish, false) => PublishReceived(publish),
            _ => throw new InvalidOperationException("Should never happen")
        };
    }

    private bool SendPublish(Publish publish)
    {
        Console.Out.WriteLine("Send Publish");
        Send(publish);
        return true;
    }

    private bool PublishReceived(Publish publish)
    {
        Console.Out.WriteLine("Publish Received");
        Console.Out.WriteLine(publish.Payload.GetString());
        return true;
    }
    
    public ClientPublishCommand(IMqttClient client, TransmissionManager transmissionManager, MessageIdentifier? messageIdentifier) : base(transmissionManager, messageIdentifier)
    {
        Client = client;
    }
}