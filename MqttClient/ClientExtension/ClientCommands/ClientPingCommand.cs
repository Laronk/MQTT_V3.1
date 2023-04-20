using MqttClient.ClientCore;
using MqttClient.ClientCore.Commands;
using MqttDataStructures;
using MqttDataStructures.Messages.Pings;

namespace MqttClient.ClientExtension.ClientCommands;

public class ClientPingCommand : Command, IClientCommand
{
    public IMqttClient Client { get; }

    public override bool Execute(Message message, bool isStart)
    {
        return message switch
        {
            PingReq pingReq => PingReq(pingReq),
            PingResp iPingResp => PingResp(iPingResp),
            _ => throw new InvalidOperationException("Should never happen")
        };
    }

    private bool PingReq(PingReq pingReq)
    {
        Console.Out.WriteLine("Send pingReq");
        Send(pingReq);
        return true;
    }
    
    private bool PingResp(PingResp pingResp)
    {
        Console.Out.WriteLine("Received pingResp");
        return true;
    }
    
    public ClientPingCommand(IMqttClient client, TransmissionManager transmissionManager, MessageIdentifier? messageIdentifier)
        : base(transmissionManager, messageIdentifier)
    {
        Client = client;
    }
}