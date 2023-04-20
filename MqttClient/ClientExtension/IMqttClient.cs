using MqttDataStructures.Building.Options;

namespace MqttClient.ClientExtension;

// Simple interface exposing basic methods
public interface IMqttClient
{
    public void Connect(ConnectOptions options);
    public void Publish(PublishOptions options);
    public void Subscribe(SubscribeOptions options);
    public void Unsubscribe(UnsubscribeOptions options);
    public void PingRequest();
    public void Disconnect();

    public void SetIsAuthorized(bool isAuthorized);
    public bool IsAuthorized();
    public void CloseTcp();
}

