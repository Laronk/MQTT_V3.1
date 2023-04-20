using MqttDataStructures.Building.Options;

namespace MqttDataStructures.Building.Builders;

public class UnsubscribeOptionsBuilder : IBuilder<UnsubscribeOptions>
{
    private readonly UnsubscribeOptions _unsubscribeOptions;

    public UnsubscribeOptionsBuilder WithDup(bool dup)
    {
        _unsubscribeOptions.Dup = dup;
        return this;
    }

    public UnsubscribeOptionsBuilder WithQos(QoS qoS)
    {
        _unsubscribeOptions.QoS = qoS;
        return this;
    }

    public UnsubscribeOptionsBuilder WithMessageIdentifier(ushort messageIdentifier)
    {
        _unsubscribeOptions.MessageIdentifier = messageIdentifier;
        return this;
    }

    public UnsubscribeOptionsBuilder WithTopicsFilters(List<string> topicFilters)
    {
        _unsubscribeOptions.TopicFilters = new List<string>(topicFilters);
        return this;
    }
    
    public UnsubscribeOptions Build()
    {
        return _unsubscribeOptions;
    }

    public UnsubscribeOptionsBuilder()
    {
        _unsubscribeOptions = new UnsubscribeOptions();
    }
}