namespace MqttDataStructures.Messages.Pings;

public class PingResp : Message
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
    
    public PingResp(FixedHeader fh, byte[] data)
        : base(fh)
    {
    }

    public PingResp()
        : base(new FixedHeader(MessageType.PingResp, null, null, null, 0))
    {
    }
}