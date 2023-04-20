using MqttDataStructures;
using MqttDataStructures.Messages.Pubs;
using MqttServer.Publisher;
using MqttServer.SrvClientExtension;
using MqttServer.Utils;
using Subscription = MqttDataStructures.Messages.Subs.Subscription;

namespace MqttServer;

public class Broker : IBroker
{
    private readonly ConnectionReceiver _connectionReceiver;
    private readonly List<IMqttSrvClient> _connectedClients;
    private readonly List<TopicPublisher> _topicPublishers;
    private readonly Authenticator _authenticator;

    public bool IsClientIdValid(string clientId)
    {
        bool isTaken = _connectedClients.Any(cl => cl.ClientId() == clientId);

        return !isTaken;
    }

    public bool IsUserNameValid(string? userName)
    {
        return _authenticator.IsUserNameValid(userName);
    }

    public bool CheckCredentials(string? userName, string? userPassword)
    {
        return _authenticator.CheckCredentials(userName, userPassword);

    }

    private void AddNewClient(IMqttSrvClient client)
    {
        _connectedClients.Add(client);
    }

    private void RemoveClient(IMqttSrvClient client)
    {
        lock (_topicPublishers)
        {
            foreach (var topicPublisher in _topicPublishers
                         .Where(topicPublisher => topicPublisher.HasSubscriber((ISubscriber)client)))
            {
                topicPublisher.RemoveSubscriber((ISubscriber)client);
            }
        }
        
        _connectedClients.Remove(client);
    }

    public void Start()
    {
        _connectionReceiver.StartListening(this, AddNewClient, RemoveClient);
    }

    public bool NewSubscription(ISubscriber subscriber, Subscription filter)
    {
        lock (_topicPublishers)
        {
            List<TopicPublisher> topicPublishers = _topicPublishers
                .Where(tp => tp.HasFilter(filter))
                .ToList();

            if (topicPublishers.Count == 0)
            {
                TopicPublisher topicPublisher = new TopicPublisher(filter);
                topicPublisher.AddSubscriber(subscriber);
                _topicPublishers.Add(topicPublisher);
            }
            else if (topicPublishers.Count == 1)
            {
                topicPublishers[0].AddSubscriber(subscriber);
            }
            else
            {
                throw new InvalidOperationException("Should never happen");
            }
        }

        return true;
    }

    public bool RemoveSubscription(ISubscriber subscriber, string filter)
    {
        Subscription subFilter = new Subscription(filter, QoS.FireAndForget);
        
        lock (_topicPublishers)
        {
            List<TopicPublisher> topicPublishers = _topicPublishers
                .Where(tp => tp.HasFilter(subFilter) && tp.HasSubscriber(subscriber))
                .ToList();

            switch (topicPublishers.Count)
            {
                case 0:
                    return true;
                case 1:
                    topicPublishers[0].RemoveSubscriber(subscriber);
                    
                    if (!topicPublishers[0].HasAnySubscriber())
                    {
                        _topicPublishers.Remove(topicPublishers[0]);
                    }
                    return true;
                default:
                    throw new InvalidOperationException("Should never happen");
            }
        }
    }

    public void MakePublishToAllSubscribed(Publish publish)
    {
        IEnumerable<TopicPublisher> topicPublishers;

        lock (_topicPublishers)
        {
            topicPublishers = _topicPublishers
                .Where(tp => tp.MatchFilter(publish.VarHead.TopicName));
        }

        foreach (TopicPublisher topicPublisher in topicPublishers)
        {
            topicPublisher.NotifySubscribers(publish);
        }
    }

    public Broker(int port, string host, List<Account> accounts)
    {
        _connectionReceiver = new ConnectionReceiver(port, host);
        _connectedClients = new List<IMqttSrvClient>();
        _topicPublishers = new List<TopicPublisher>();
        _authenticator = new Authenticator(accounts);
    }
}