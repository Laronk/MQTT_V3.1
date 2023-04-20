using MqttClient.ClientCore;
using MqttClient.ClientCore.Commands;
using MqttDataStructures;

namespace MqttClient.ClientExtension.ClientCommands;

public class ClientCommandFactory : ICommandFactory
{
    private readonly IMqttClient _client;

    public Command CreateCommand(Message m, TransmissionManager transmissionManager)
    {
        return m.FixedHeader.MessageType switch
        {
            MessageType.Connect or MessageType.ConnAck => new ClientConnectCommand(_client, transmissionManager, m.GetMessageIdentifier()),
            MessageType.Disconnect => new ClientDisconnectCommand(_client, transmissionManager, m.GetMessageIdentifier()),
            MessageType.PingReq or MessageType.PingResp => new ClientPingCommand(_client, transmissionManager, m.GetMessageIdentifier()),
            MessageType.Publish => new ClientPublishCommand(_client, transmissionManager, m.GetMessageIdentifier()),
            MessageType.Subscribe or MessageType.SubAck => new ClientSubscribeCommand(_client, transmissionManager, m.GetMessageIdentifier()),
            MessageType.Unsubscribe or MessageType.UnsubAck => new ClientUnsubscribeCommand(_client, transmissionManager, m.GetMessageIdentifier()),
            _ => throw new Exception("Should never happen")
        };
    }

    public ClientCommandFactory(IMqttClient client)
    {
        _client = client;
    }
}