using MqttClient.ClientCore;
using MqttClient.ClientCore.Commands;
using MqttDataStructures;
using MqttDataStructures.Messages.UnSubs;

namespace MqttServer.SrvClientExtension.SrvCommands;

public class SrvUnsubscribeCommand : Command, ISrvCommand
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
            Unsubscribe unsubscribe => Unsubscribe(unsubscribe),
            _ => throw new InvalidOperationException("Should never happen")
        };
    }

    private bool Unsubscribe(Unsubscribe unsubscribe)
    {
        Console.Out.WriteLine("Received Unsubscribe");
        Client.RemoveSubscription(unsubscribe);
        Send(new UnsubAck(unsubscribe.GetMessageIdentifier()!.Value));
        Console.Out.WriteLine("Send UnSubAck");
        return true;
    }

    public SrvUnsubscribeCommand(IMqttSrvClient client, TransmissionManager transmissionManager, MessageIdentifier? messageIdentifier) : base(transmissionManager, messageIdentifier)
    {
        Client = client;
    }
}