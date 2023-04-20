using MqttDataStructures.Building.Builders;

namespace MqttClient.ClientExtension;

public sealed class ClientFactory
{
    public IMqttClient CreateClient()
    {
        return new MqttClient();
    }
    
    public ConnectOptionsBuilder ConnectOptionsBuilder()
    {
        return new ConnectOptionsBuilder();
    }

    public PublishOptionsBuilder PublishOptionsBuilder()
    {
        return new PublishOptionsBuilder();
    }
    
    public SubscribeOptionsBuilder SubscribeOptionsBuilder()
    {
        return new SubscribeOptionsBuilder();
    }

    public UnsubscribeOptionsBuilder UnsubscribeOptionsBuilder()
    {
        return new UnsubscribeOptionsBuilder();
    }
}