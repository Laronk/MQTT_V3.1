using MqttClient.ClientCore;
using MqttClient.ClientCore.Commands;
using MqttDataStructures;
using MqttDataStructures.Messages.UnSubs;

namespace MqttClient.ClientExtension.ClientCommands;

public class ClientUnsubscribeCommand : Command, IClientCommand
{
    public IMqttClient Client { get; }
    private volatile bool _stop;

    public override bool Execute(Message message, bool isStart)
    {
        return message switch
        {
            Unsubscribe unsubscribe => Unsubscribe(unsubscribe),
            UnsubAck unsubAck => UnsubAck(unsubAck),
            _ => throw new InvalidOperationException("Should never happen")
        };
    }

    private bool Unsubscribe(Unsubscribe unsubscribe)
    {
        Console.Out.WriteLine("Send Unsubscribe");
        Send(unsubscribe);
        unsubscribe.FixedHeader.Dup = true;
        new Task(() =>
        {
            while (true)
            {
                Thread.Sleep(5000);
                if (_stop)
                {
                    return;
                }
                Send(unsubscribe);
            }
        }).Start();
        return false;
    }

    private bool UnsubAck(UnsubAck unsubAck)
    {
        Console.Out.WriteLine("Received unsubAck");
        _stop = true;
        return true;
    }

    public ClientUnsubscribeCommand(IMqttClient client, TransmissionManager transmissionManager,
        MessageIdentifier? messageIdentifier)
        : base(transmissionManager, messageIdentifier)
    {
        _stop = false;
        Client = client;
    }
}