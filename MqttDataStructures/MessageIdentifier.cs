namespace MqttDataStructures;

public class MessageIdentifier : IGetBytes
{
    public ushort Value { get; }

    public List<byte> GetBytes()
    {
        return new List<byte>
        {
            (byte)(Value >> 8), (byte)(Value & 0xFF)
        };
    }
    
    public MessageIdentifier(byte[] bytes, out byte[] remainingBytes)
    {
        Value = (ushort)((bytes[0] << 8) + bytes[1]);
        remainingBytes = bytes.Skip(2).ToArray();
    }
    
    public MessageIdentifier(ushort value)
    {
        Value = value;
    }
}