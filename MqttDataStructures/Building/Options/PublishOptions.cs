namespace MqttDataStructures.Building.Options;

public class PublishOptions : IOptions
{
    public QoS QoS { get; set; }
    public bool Dup { get; set; }
    public bool Retain { get; set; }
    public string TopicName { get; set; }
    public ushort MessageIdentifier { get; set; }
    public byte[] Payload { get; set; }

    public PublishOptions()
    {
        QoS = QoS.FireAndForget;
        Dup = false;
        Retain = false;
        TopicName = "";
        MessageIdentifier = 1;
        Payload = Array.Empty<byte>();
    }
}