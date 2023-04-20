namespace MqttDataStructures.Messages.Subs;

public class SubAck : Message
{
    private readonly IVariableHeader _variableHeader;
    private readonly IPayload _payload;

    public SubAckVarHead VariableHeader =>
        _variableHeader as SubAckVarHead ??
        throw new InvalidOperationException("Should never happen");

    public SubAckPayload Payload =>
        _payload as SubAckPayload ??
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

    public SubAck(FixedHeader fh, byte[] data)
        : base(fh)
    {
        _variableHeader = new SubAckVarHead(data, out data);
        _payload = new SubAckPayload(data);
    }

    public SubAck(ushort messageIdentifier, List<QoS> grantedQosLevels)
        : base(new FixedHeader(MessageType.SubAck, null, null, null, 0))
    {
        _variableHeader = new SubAckVarHead(messageIdentifier);
        _payload = new SubAckPayload(grantedQosLevels);
        
        SetRemainingLength();
    }
}