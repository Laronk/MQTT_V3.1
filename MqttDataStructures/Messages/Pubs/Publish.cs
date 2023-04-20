using MqttDataStructures.Building.Options;

namespace MqttDataStructures.Messages.Pubs;

public class Publish : Message
{
    private readonly IVariableHeader _variableHeader;
    private readonly IPayload _payload;
    
    public PublishVarHead VarHead => 
        _variableHeader as PublishVarHead ??
        throw new InvalidOperationException("Should never happen");

    public PublishPayload Payload => 
        _payload as PublishPayload ??
        throw new InvalidOperationException("Should never happen");
    
    public override MessageIdentifier? GetMessageIdentifier()
    {
        return _variableHeader.MessageIdentifier;
    }

    public override void SetMessageIdentifier(ushort messageIdentifier)
    {
        _variableHeader.MessageIdentifier = new MessageIdentifier(messageIdentifier);
        SetRemainingLength();
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
    
    public Publish(FixedHeader fh, byte[] data)
        : base(fh)
    {
        _variableHeader = new PublishVarHead(fh, data, out data);
        _payload = new PublishPayload(data);
    }

    public Publish(PublishOptions options)
        : base(new FixedHeader(MessageType.Publish, options.Dup, options.QoS, options.Retain, 0))
    {
        _variableHeader = new PublishVarHead(options);
        _payload = new PublishPayload(options.Payload);
        
        SetRemainingLength();
    }
}