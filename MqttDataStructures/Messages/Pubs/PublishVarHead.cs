using MqttDataStructures.Building.Options;
using MqttDataStructures.Utils;

namespace MqttDataStructures.Messages.Pubs;

public class PublishVarHead : IVariableHeader
{
    private MessageIdentifier? _messageIdentifier;
    public string TopicName { get; }
    
    public MessageIdentifier? MessageIdentifier
    {
        get => _messageIdentifier;
        set => _messageIdentifier =
            value ??
            throw new ArgumentNullException(nameof(value));
    }

    public List<byte> GetBytes()
    {
        List<byte> result = new List<byte>();
        result.AddRange(BytesOperator.StringToBytes(TopicName));

        if (_messageIdentifier is not null)
        {
            result.AddRange(_messageIdentifier.GetBytes());
        }
        return result;
    }

    public PublishVarHead(FixedHeader fh, byte[] bytes, out byte[] remainingBytes)
    {
        TopicName = BytesOperator.GetStringFromStream(bytes, out remainingBytes);
        _messageIdentifier = fh.Qos is QoS.FireAndForget
            ? null
            : new MessageIdentifier(remainingBytes, out remainingBytes);
    }

    public PublishVarHead(PublishOptions options)
    {
        _messageIdentifier = options.QoS is QoS.FireAndForget
            ? null
            : new MessageIdentifier(options.MessageIdentifier);
        TopicName = options.TopicName;
    }
}