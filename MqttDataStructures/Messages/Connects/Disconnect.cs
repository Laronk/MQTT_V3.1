namespace MqttDataStructures.Messages.Connects;

public sealed class Disconnect : Message
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
    
    public Disconnect(FixedHeader fh, byte[] data)
        : base(fh)
    {
    }

    public Disconnect()
        : base(new FixedHeader(MessageType.Disconnect, null, null, null, 0))
    {
    }
}