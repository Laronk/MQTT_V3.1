using MqttClient.ClientCore;
using MqttClient.ClientCore.Commands;
using MqttDataStructures;
using MqttDataStructures.Messages.Pings;

namespace MqttServer.SrvClientExtension.SrvCommands;

public class SrvPingCommand : Command, ISrvCommand
{
    public IMqttSrvClient Client { get; }

    public override bool Execute(Message message, bool isStart)
    {
        return message switch
        {
            PingReq pingReq => PingReq(pingReq),
            _ => throw new InvalidOperationException("Should never happen")
        };
    }

    private bool PingReq(PingReq pingReq)
    {
        Console.Out.WriteLine("Received pingReq");
        Console.Out.WriteLine("Send pingResp");
        Send(new PingResp());
        return true;
    }

    public SrvPingCommand(IMqttSrvClient client, TransmissionManager transmissionManager, MessageIdentifier? messageIdentifier) : base(transmissionManager,
        messageIdentifier)
    {
        Client = client;
    }
}