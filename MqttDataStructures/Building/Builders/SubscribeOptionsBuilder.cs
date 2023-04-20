using MqttDataStructures.Building.Options;
using MqttDataStructures.Messages.Subs;

namespace MqttDataStructures.Building.Builders;

public class SubscribeOptionsBuilder : IBuilder<SubscribeOptions>
{
    private readonly SubscribeOptions _subscribeOptions;

    public SubscribeOptionsBuilder WithDup(bool dup)
    {
        _subscribeOptions.Dup = dup;
        return this;
    }

    public SubscribeOptionsBuilder WithMessageIdentifier(ushort messageIdentifier)
    {
        _subscribeOptions.MessageIdentifier = messageIdentifier;
        return this;
    }
    
    public SubscribeOptionsBuilder WithTopicFilters(List<Tuple<string, QoS>> topicFilters)
    {
        _subscribeOptions.Topics = topicFilters
            .Select(top => new Subscription(top.Item1, top.Item2))
            .ToList();

        return this;
    }
    
    public SubscribeOptions Build()
    {
        return _subscribeOptions;
    }

    public SubscribeOptionsBuilder()
    {
        _subscribeOptions = new SubscribeOptions();
    }
}