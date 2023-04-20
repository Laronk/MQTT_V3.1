using MqttClient.ClientCore;
using MqttClient.ClientCore.Commands;
using MqttDataStructures;
using MqttDataStructures.Messages.Connects;

namespace MqttServer.SrvClientExtension.SrvCommands;

public class SrvDisconnectCommand : Command, ISrvCommand
{
    public IMqttSrvClient Client { get; }

    public override bool Execute(Message message, bool isStart)
    {
        return (message, isStart) switch
        {
            (Disconnect disconnect, false) => Disconnect(disconnect),
            (Disconnect disconnect, true) => DisconnectBroken(disconnect),
            _ => throw new InvalidOperationException("Should never happen")
        };
    }
    
    private bool Disconnect(Disconnect disconnect)
    {
        Console.Out.WriteLine($"Received Disconnect from {Client.ClientId()}");
        Client.RemoveClient();
        return true;
    }
    
    private bool DisconnectBroken(Disconnect disconnect)
    {
        if (Client.ClientId() is not null)
        {
            Client.ProcedureLastWill();
        }
        Console.Out.WriteLine($"Disconnected client {Client.ClientId()}");
        Client.RemoveClient();
        return true;
    }

    public SrvDisconnectCommand(IMqttSrvClient client, TransmissionManager transmissionManager, MessageIdentifier? messageIdentifier) : base(transmissionManager,
        messageIdentifier)
    {
        Client = client;
    }
}