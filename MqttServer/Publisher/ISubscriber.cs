using MqttDataStructures.Messages.Pubs;

namespace MqttServer.Publisher;

public interface ISubscriber
{
    public void GotPublish(Publish publish);
    public string GetSubscriberId();
}