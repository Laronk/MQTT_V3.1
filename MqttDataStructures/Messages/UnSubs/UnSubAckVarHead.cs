namespace MqttDataStructures.Messages.UnSubs;

public class UnSubAckVarHead : IVariableHeader
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

    public UnSubAckVarHead(byte[] bytes)
    {
        _messageIdentifier = new MessageIdentifier(bytes, out _);
    }
    
    public UnSubAckVarHead(ushort messageIdentifier)
    {
        _messageIdentifier = new MessageIdentifier(messageIdentifier);
    }
}