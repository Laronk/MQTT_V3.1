namespace MqttDataStructures.Messages.Connects;

public class ConnAck : Message
{
    private readonly IVariableHeader _variableHeader;

    public ConnAckVarHead VariableHeader =>
        _variableHeader as ConnAckVarHead ??
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

    public ConnAck(FixedHeader fh, byte[] data) : base(fh)
    {
        _variableHeader = new ConnAckVarHead(data);
    }

    public ConnAck(ConnectReturnCode returnCode)
        : base(new FixedHeader(MessageType.ConnAck, null, null, null, 0))
    {
        _variableHeader = new ConnAckVarHead(returnCode);
        SetRemainingLength();
    }
}