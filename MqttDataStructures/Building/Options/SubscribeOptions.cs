using MqttDataStructures.Messages.Subs;

namespace MqttDataStructures.Building.Options;

public class SubscribeOptions : IOptions
{
    public bool Dup { get; set; }
    public QoS Qos { get; }
    public ushort MessageIdentifier { get; set; }
    public List<Subscription> Topics { get; set; }

    public SubscribeOptions()
    {
        Dup = false;
        Qos = QoS.AcknowledgedDelivery;
        MessageIdentifier = 1;
        Topics = new List<Subscription>();
    }
}