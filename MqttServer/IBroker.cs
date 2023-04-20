using MqttDataStructures.Messages.Pubs;
using MqttServer.Publisher;
using Subscription = MqttDataStructures.Messages.Subs.Subscription;

namespace MqttServer;

public interface IBroker
{
    public void Start();
    public bool IsClientIdValid(string clientId);
    public bool IsUserNameValid(string? userName);
    public bool CheckCredentials(string? userName, string? userPassword);
    public bool NewSubscription(ISubscriber subscriber, Subscription filter);
    public bool RemoveSubscription(ISubscriber subscriber, string filter);
    public void MakePublishToAllSubscribed(Publish publish);
}