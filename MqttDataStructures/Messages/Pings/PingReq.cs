namespace MqttDataStructures.Messages.Pings;

public class PingReq : Message
{
    public override MessageIdentifier? GetMessageIdentifier()
    {
        return null;
    }
    
    public override void SetMessageIdentifier(ushort messageIdentifier)
    {
        throw new InvalidOperationException("Should never happen");
    }

    protected override void SetRemainingLength()
    {
        FixedHeader.RemainingLength = 0;
    }

    public override List<byte> GetBytes()
    {
        return FixedHeader.GetBytes();
    }
    
    public PingReq(FixedHeader fh, byte[] data)
        : base(fh)
    {
    }

    public PingReq()
        : base(new FixedHeader(MessageType.PingReq, null, null, null, 0))
    {
    }
}