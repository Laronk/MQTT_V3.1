using MessageConverter;
using MqttDataStructures.Messages.Pubs;
using MqttDataStructures.Messages.Subs;

namespace MqttServer.Publisher;

public class TopicPublisher
{
    private readonly Subscription _filter;
    private readonly HashSet<ISubscriber> _subscribers;

    public bool HasFilter(Subscription filter)
    {
        return _filter.Topic == filter.Topic;
    }

    public bool HasSubscriber(ISubscriber subscriber)
    {
        return _subscribers
            .Select(s => s.GetSubscriberId())
            .Any(id => id == subscriber.GetSubscriberId());
    }
    
    public bool HasAnySubscriber()
    {
        return _subscribers.Count > 0;
    }

    public bool MatchFilter(string topicName)
    {
        if (FilterComparer.Compare(topicName, _filter.Topic) is FilterCompareResult.IsMatch)
        {
            return true;
        }

        return false;
    }

    public void AddSubscriber(ISubscriber subscriber)
    {
        if (_subscribers
            .Select(s => s.GetSubscriberId())
            .Contains(subscriber.GetSubscriberId())
           )
        {
            return;
        }

        _subscribers.Add(subscriber);
    }

    public void RemoveSubscriber(ISubscriber subscriber)
    {
        _subscribers.Remove(subscriber);
    }

    public void NotifySubscribers(Publish publish)
    {
        foreach (ISubscriber subscriber in _subscribers)
        {
            new Task(() =>
            {
                subscriber.GotPublish(Converter.ConvertToMessage(
                                          publish.GetBytes().ToArray(),
                                          out int consumed,
                                          out bool corrupted
                                      ) as Publish
                                      ?? throw new InvalidOperationException("Should never happen")
                );
            }).Start();
        }
    }

    public TopicPublisher(Subscription subscription)
    {
        _filter = subscription;
        _subscribers = new HashSet<ISubscriber>();
    }
}