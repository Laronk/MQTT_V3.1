namespace MqttDataStructures.Messages.UnSubs;

public class UnSubscribeVarHead : IVariableHeader
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

    public UnSubscribeVarHead(byte[] bytes, out byte[] remainingData)
    {
        _messageIdentifier = new MessageIdentifier(bytes, out remainingData);
    }
    
    public UnSubscribeVarHead(ushort messageIdentifier)
    {
        _messageIdentifier = new MessageIdentifier(messageIdentifier);
    }
}