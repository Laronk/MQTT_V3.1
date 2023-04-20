using MqttClient.ClientCore;
using MqttClient.ClientCore.Commands;
using MqttDataStructures;
using MqttDataStructures.Messages.Subs;

namespace MqttServer.SrvClientExtension.SrvCommands;

public class SrvSubscribeCommand : Command, ISrvCommand
{
    public IMqttSrvClient Client { get; }

    public override bool Execute(Message message, bool isStart)
    {
        if (Client.ClientId() == null)
        {
            return true;
        }

        return message switch
        {
            Subscribe subscribe => Subscribe(subscribe),
            _ => throw new InvalidOperationException("Should never happen")
        };
    }

    private bool Subscribe(Subscribe subscribe)
    {
        Console.Out.WriteLine("Received Subscribe");
        Client.NewSubscription(subscribe);
        Send(new SubAck(
                subscribe.GetMessageIdentifier()!.Value,
                subscribe.Payload.Subscriptions
                    .Select(sub => sub.Qos)
                    .ToList()
            )
        );
        Console.Out.WriteLine("Send SubAck");
        return true;
    }

    public SrvSubscribeCommand(IMqttSrvClient client, TransmissionManager transmissionManager,
        MessageIdentifier? messageIdentifier) : base(transmissionManager,
        messageIdentifier)
    {
        Client = client;
    }
}