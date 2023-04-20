using MqttDataStructures.Building.Options;

namespace MqttDataStructures.Messages.Connects;

public sealed class Connect : Message
{
    private const string ProtocolName = "MQIsdp";
    private const ushort ProtocolVersionNumber = 3;
    private const ushort KeepAliveTimer = 120;

    private readonly IVariableHeader _variableHeader;
    private readonly IPayload _payload;

    public ConnectVarHead VariableHeader =>
        _variableHeader as ConnectVarHead ??
        throw new InvalidOperationException("Should never happen");

    public ConnectPayload Payload =>
        _payload as ConnectPayload ??
        throw new InvalidOperationException("Should never happen");

    public override MessageIdentifier? GetMessageIdentifier()
    {
        return _variableHeader.MessageIdentifier;
    }
    
    public override void SetMessageIdentifier(ushort messageIdentifier)
    {
        _variableHeader.MessageIdentifier = new MessageIdentifier(messageIdentifier);
    }

    protected override void SetRemainingLength()
    {
        FixedHeader.RemainingLength = _variableHeader.BytesConsumed + _payload.BytesConsumed;
    }

    public override List<byte> GetBytes()
    {
        List<byte> result = new List<byte>();
        result.AddRange(FixedHeader.GetBytes());
        result.AddRange(_variableHeader.GetBytes());
        result.AddRange(_payload.GetBytes());
        return result;
    }

    public Connect(FixedHeader fh, byte[] data) : base(fh)
    {
        _variableHeader = new ConnectVarHead(data, out data);
        _payload = new ConnectPayload(data,
            _variableHeader as ConnectVarHead ??
            throw new InvalidOperationException("Should never happen"));
    }

    public Connect(ConnectOptions options)
        : base(new FixedHeader(MessageType.Connect, null, null, null, null))
    {
        ConnectFlags connectFlags = new ConnectFlags(
            options.UserName is not null,
            options.Password is not null,
            options.CleanSession,
            options.WillTopic is not null && options.WillMessage is not null,
            options.WillQos,
            options.WillRetain
        );

        _variableHeader = new ConnectVarHead(
            ProtocolName,
            ProtocolVersionNumber,
            connectFlags,
            KeepAliveTimer
        );

        _payload = new ConnectPayload(options);
        SetRemainingLength();
    }
}