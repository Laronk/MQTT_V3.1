namespace MqttDataStructures;

public class FixedHeader : IGetBytes
{
    public int BytesConsumed { get; }

    public MessageType MessageType { get; }
    public bool? Dup { get; set; }
    public QoS? Qos { get; }
    public bool? Retain { get; }
    public int? RemainingLength { get; set; }

    public List<byte> GetBytes()
    {
        if (RemainingLength is not {} x)
        {
            throw new InvalidOperationException("Should never happen");
        }
        
        List<byte> result = new List<byte>();
        int result1 = 0;
        result1 += ((byte)MessageType) << 4;
        result1 += Dup.HasValue? Dup.Value ? 1 << 3 : 0 : 0;
        result1 += Qos.HasValue? (((int)Qos.Value) << 1) : 0;
        result1 += Retain.HasValue? Retain.Value ? 1 : 0 : 0;
        result.Add((byte)result1);
        
        do
        {
            int digit = x % 128;
            x = x / 128;
            // if there are more digits to encode, set the top bit of this digit
            if (x > 0)
                digit |= 128;
            result.Add((byte)digit);
        } while (x > 0);

        return result;
    }
    
    public FixedHeader(byte[] message)
    {
        BytesConsumed = 0;

        byte headByte = message[BytesConsumed++];
        MessageType = (MessageType)(headByte >> 4);
        Dup = (headByte & (1 << 3)) != 0;
        Qos = (QoS)((headByte & (6)) >> 1);
        Retain = (headByte & 1) != 0;
        BytesConsumed = 1;
        RemainingLength = 0;
        
        int multiplier = 1;
        byte digit;
        do
        {
            digit = message[BytesConsumed++];
            RemainingLength += (digit & 127) * multiplier;
            multiplier *= 128;
        } while ((digit & 128) != 0);
    }

    public FixedHeader(MessageType messageType, bool? dup, QoS? qos, bool? retain, int? remainingLength)
    {
        MessageType = messageType;
        Dup = dup;
        Qos = qos;
        Retain = retain;
        RemainingLength = remainingLength;
    }
}