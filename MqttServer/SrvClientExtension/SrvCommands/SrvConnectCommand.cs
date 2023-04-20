using MqttClient.ClientCore;
using MqttClient.ClientCore.Commands;
using MqttDataStructures;
using MqttDataStructures.Building.Builders;
using MqttDataStructures.Messages.Connects;
using MqttDataStructures.Messages.Pubs;

namespace MqttServer.SrvClientExtension.SrvCommands;

public class SrvConnectCommand : Command, ISrvCommand
{
    public IMqttSrvClient Client { get; }

    public override bool Execute(Message message, bool isStart)
    {
        if (Client.ClientId() is { } clientId) return true;

        return message switch
        {
            Connect connect => Connect(connect),
            _ => throw new InvalidOperationException("Should never happen")
        };
    }

    private bool Connect(Connect connect)
    {
        Console.Out.WriteLine($"Received Connect with Id: {connect.Payload.ClientId}");
        if (Client.ClientId() != null) return true;

        if (!ClientIdValid(connect)) return true;
        if (!ProtocolVersionNumber(connect)) return true;
        if (!BadUserNameOrPassword(connect)) return true;
        if (!NotAuthorized(connect)) return true;

        CheckLastWill(connect);

        Console.Out.WriteLine($"Send ConnAck with {ConnectReturnCode.ConnectionAccepted}");
        Send(new ConnAck(ConnectReturnCode.ConnectionAccepted));
        return true;
    }

    private void CheckLastWill(Connect connect)
    {
        if (!connect.VariableHeader.ConnectFlags.WillFlag) return;
        if (connect.VariableHeader.ConnectFlags.WillQos == null) return;
        
        Publish publish = new Publish(new PublishOptionsBuilder()
            .WithQoS((QoS)connect.VariableHeader.ConnectFlags.WillQos)
            .WithTopicName(connect.Payload.WillTopic!)
            .WithPayload(connect.Payload.WillMessage!)
            .Build());

        Client.SaveLastWill(publish);
    }
    
    private bool ClientIdValid(Connect connect)
    {
        if (Client.TrySetClientId(connect.Payload.ClientId)) return true;

        Console.Out.WriteLine($"Send ConnAck with {ConnectReturnCode.ConnectionRefusedIdentifierRejected}");
        Send(new ConnAck(ConnectReturnCode.ConnectionRefusedIdentifierRejected));
        Client.RemoveClient();
        return false;
    }

    private bool ProtocolVersionNumber(Connect connect)
    {
        if (connect.VariableHeader.ProtocolVersionNumber == 3) return true;

        Console.Out.WriteLine($"Send ConnAck with {ConnectReturnCode.ConnectionRefusedUnacceptableProtocolVersion}");
        Send(new ConnAck(ConnectReturnCode.ConnectionRefusedUnacceptableProtocolVersion));
        Client.RemoveClient();
        return false;
    }

    private bool BadUserNameOrPassword(Connect connect)
    {
        if (Client.CheckUserName(connect.Payload.Username)) return true;

        Console.Out.WriteLine($"Send ConnAck with {ConnectReturnCode.ConnectionRefusedBadUserNameOrPassword}");
        Send(new ConnAck(ConnectReturnCode.ConnectionRefusedBadUserNameOrPassword));
        Client.RemoveClient();
        return false;
    }

    private bool NotAuthorized(Connect connect)
    {
        if (Client.CheckUserCredentials(connect.Payload.Username, connect.Payload.Password)) return true;

        Console.Out.WriteLine($"Send ConnAck with {ConnectReturnCode.ConnectionRefusedNotAuthorized}");
        Send(new ConnAck(ConnectReturnCode.ConnectionRefusedNotAuthorized));
        Client.RemoveClient();
        return false;
    }

    public SrvConnectCommand(IMqttSrvClient client, TransmissionManager transmissionManager,
        MessageIdentifier? messageIdentifier)
        : base(transmissionManager, messageIdentifier)
    {
        Client = client;
    }
}