using MqttClient.ClientCore;
using MqttClient.ClientCore.Commands;
using MqttDataStructures;
using MqttDataStructures.Messages.Pubs;

namespace MqttServer.SrvClientExtension.SrvCommands;

public class SrvPublishCommand : Command, ISrvCommand
{
    public IMqttSrvClient Client { get; }

    public override bool Execute(Message message, bool isStart)
    {
        if (Client.ClientId() == null)
        {
            return true;
        }
        
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
        Client.MakePublishToAllSubscribed(publish);
        return true;
    }

    public SrvPublishCommand(IMqttSrvClient client, TransmissionManager transmissionManager, MessageIdentifier? messageIdentifier) : base(transmissionManager,
        messageIdentifier)
    {
        Client = client;
    }
}