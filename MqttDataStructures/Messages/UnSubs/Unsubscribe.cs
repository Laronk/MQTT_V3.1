using MqttDataStructures.Building.Options;

namespace MqttDataStructures.Messages.UnSubs;

public class Unsubscribe : Message
{
    private readonly IVariableHeader _variableHeader;
    private readonly IPayload _payload;
    
    public UnSubscribeVarHead VarHead =>
        _variableHeader as UnSubscribeVarHead ??
        throw new InvalidOperationException("Should never happen");

    public UnSubscribePayload Payload =>
        _payload as UnSubscribePayload ??
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
    
    public Unsubscribe(FixedHeader fh, byte[] data)
        : base(fh)
    {
        _variableHeader = new UnSubscribeVarHead(data, out data);
        _payload = new UnSubscribePayload(data);
    }

    public Unsubscribe(UnsubscribeOptions options)
        : base(new FixedHeader(MessageType.Unsubscribe, false, QoS.AcknowledgedDelivery, null, 0))
    {
        _variableHeader = new UnSubscribeVarHead(options.MessageIdentifier);
        _payload = new UnSubscribePayload(options.TopicFilters);

        SetRemainingLength();
    }
}