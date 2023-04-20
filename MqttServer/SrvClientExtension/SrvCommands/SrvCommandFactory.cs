using MqttClient.ClientCore;
using MqttClient.ClientCore.Commands;
using MqttDataStructures;

namespace MqttServer.SrvClientExtension.SrvCommands;

public class SrvCommandFactory : ICommandFactory
{
    private readonly IMqttSrvClient _client;

    public Command CreateCommand(Message m, TransmissionManager transmissionManager)
    {
        return m.FixedHeader.MessageType switch
        {
            MessageType.Connect or MessageType.ConnAck => new SrvConnectCommand(_client, transmissionManager, m.GetMessageIdentifier()),
            MessageType.Disconnect => new SrvDisconnectCommand(_client, transmissionManager, m.GetMessageIdentifier()),
            MessageType.PingReq or MessageType.PingResp => new SrvPingCommand(_client, transmissionManager, m.GetMessageIdentifier()),
            MessageType.Publish => new SrvPublishCommand(_client, transmissionManager, m.GetMessageIdentifier()),
            MessageType.Subscribe or MessageType.SubAck => new SrvSubscribeCommand(_client, transmissionManager, m.GetMessageIdentifier()),
            MessageType.Unsubscribe or MessageType.UnsubAck => new SrvUnsubscribeCommand(_client, transmissionManager, m.GetMessageIdentifier()),
            _ => throw new Exception("Should never happen")
        };
    }

    public SrvCommandFactory(IMqttSrvClient client)
    {
        _client = client;
    }
}