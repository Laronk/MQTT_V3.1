using MqttDataStructures.Building.Options;

namespace MqttDataStructures.Messages.Subs;

public class Subscribe : Message
{
    private readonly IVariableHeader _variableHeader;
    private readonly IPayload _payload;

    public SubscribeVarHead VarHead =>
        _variableHeader as SubscribeVarHead ??
        throw new InvalidOperationException("Should never happen");

    public SubscribePayload Payload =>
        _payload as SubscribePayload ??
        throw new InvalidOperationException("Should never happen");

    public override MessageIdentifier? GetMessageIdentifier()
    {
        return _variableHeader.MessageIdentifier;
    }

    public override void SetMessageIdentifier(ushort messageIdentifier)
    {
        _variableHeader.MessageIdentifier = new MessageIdentifier(messageIdentifier);
    }
    
    protected sealed override void SetRemainingLength()
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

    public Subscribe(FixedHeader fh, byte[] data)
        : base(fh)
    {
        _variableHeader = new SubscribeVarHead(data, out data);
        _payload = new SubscribePayload(data);
    }

    public Subscribe(SubscribeOptions options)
        : base(new FixedHeader(MessageType.Subscribe, options.Dup, options.Qos, null, null))
    {
        _variableHeader = new SubscribeVarHead(options.MessageIdentifier);
        _payload = new SubscribePayload(options);

        SetRemainingLength();
    }
}