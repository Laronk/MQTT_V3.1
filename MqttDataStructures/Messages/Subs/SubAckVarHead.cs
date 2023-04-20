namespace MqttDataStructures.Messages.Subs;

public class SubAckVarHead : IVariableHeader
{
    private MessageIdentifier _messageIdentifier;

    public MessageIdentifier? MessageIdentifier
    {
        get => _messageIdentifier;
        set => _messageIdentifier =
            value ??
            throw new ArgumentNullException(nameof(value));
    }

    public List<byte> GetBytes()
    {
        return _messageIdentifier.GetBytes();
    }

    public SubAckVarHead(byte[] bytes, out byte[] remainingBytes)
    {
        _messageIdentifier = new MessageIdentifier(bytes, out remainingBytes);
    }
    
    public SubAckVarHead(ushort messageIdentifier)
    {
        _messageIdentifier = new MessageIdentifier(messageIdentifier);
    }
}