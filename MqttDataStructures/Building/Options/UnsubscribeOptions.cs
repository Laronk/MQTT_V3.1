namespace MqttDataStructures.Building.Options;

public class UnsubscribeOptions :IOptions
{
    public bool Dup { get; set; }
    public QoS QoS { get; set; }
    public ushort MessageIdentifier { get; set; }
    public List<string> TopicFilters { get; set; }

    public UnsubscribeOptions()
    {
        Dup = false;
        QoS = QoS.AcknowledgedDelivery;
        MessageIdentifier = 1;
        TopicFilters = new List<string>();
    }
}