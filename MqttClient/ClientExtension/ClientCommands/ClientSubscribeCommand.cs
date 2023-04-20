using MqttClient.ClientCore;
using MqttClient.ClientCore.Commands;
using MqttDataStructures;
using MqttDataStructures.Messages.Subs;

namespace MqttClient.ClientExtension.ClientCommands;

public class ClientSubscribeCommand : Command, IClientCommand
{
    public IMqttClient Client { get; }
    
    public override bool Execute(Message message, bool isStart)
    {
        return message switch
        {
            Subscribe subscribe => Subscribe(subscribe),
            SubAck subAck => SubAck(subAck),
            _ => throw new InvalidOperationException("Should never happen")
        };
    }
    
    private bool Subscribe(Subscribe subscribe)
    {
        Console.Out.WriteLine("Send Subscribe");
        Send(subscribe);
        return false;
    }

    private bool SubAck(SubAck subAck)
    {
        Console.Out.WriteLine("Received subAck");
        return true;
    }

    public ClientSubscribeCommand(IMqttClient client, TransmissionManager transmissionManager, MessageIdentifier? messageIdentifier)
        : base(transmissionManager, messageIdentifier)
    {
        Client = client;
    }
}