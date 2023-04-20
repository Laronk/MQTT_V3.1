namespace MqttDataStructures.Messages.UnSubs;

public class UnsubAck : Message
{
    private readonly IVariableHeader _variableHeader;

    public UnSubAckVarHead VarHead =>
        _variableHeader as UnSubAckVarHead ??
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
        FixedHeader.RemainingLength = _variableHeader.BytesConsumed;
    }

    public override List<byte> GetBytes()
    {
        List<byte> result = new List<byte>();
        result.AddRange(FixedHeader.GetBytes());
        result.AddRange(_variableHeader.GetBytes());
        return result;
    }

    public UnsubAck(FixedHeader fh, byte[] data)
        : base(fh)
    {
        _variableHeader = new UnSubAckVarHead(data);
    }
    
    public UnsubAck(ushort messageIdentifier)
        : base(new FixedHeader(MessageType.UnsubAck, null, null, null, 0))
    {
        _variableHeader = new UnSubAckVarHead(messageIdentifier);
        SetRemainingLength();
    }
}