using MqttDataStructures.Utils;

namespace MqttDataStructures.Messages.Subs;

public class Subscription
{
    public string Topic { get; }
    public QoS Qos { get; }
    
    public List<byte> GetBytes()
    {
        List<byte> bytes = new List<byte>();
        bytes.AddRange(BytesOperator.StringToBytes(Topic));
        bytes.Add((byte)Qos);
        return bytes;
    }
    
    public Subscription(byte[] bytes, out byte[] outBytes)
    {
        Topic = BytesOperator.GetStringFromStream(bytes, out byte[] b1);
        Qos = (QoS)b1[0];
        outBytes = b1.Skip(1).ToArray();
    }
    
    public Subscription(string topic, QoS qos)
    {
        Topic = topic;
        Qos = qos;
    }
}