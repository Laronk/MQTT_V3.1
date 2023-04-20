using MqttDataStructures.Building.Options;

namespace MqttDataStructures.Messages.Subs;

public class SubscribePayload : IPayload
{
    private readonly List<Subscription> _subscriptions;

    public List<Subscription> Subscriptions => new(_subscriptions);

    public List<byte> GetBytes() => Subscriptions.SelectMany(x => x.GetBytes()).ToList();

    public SubscribePayload(byte[] bytes)
    {
        _subscriptions = new List<Subscription>();
        while (bytes.Length > 0)
        {
            _subscriptions.Add(new Subscription(bytes, out bytes));
        }
    }

    public SubscribePayload(SubscribeOptions options)
    {
        _subscriptions = new List<Subscription>(options.Topics);
    }
}