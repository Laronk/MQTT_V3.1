using MqttDataStructures.Building.Options;

namespace MqttDataStructures.Building.Builders;

public sealed class ConnectOptionsBuilder : IBuilder<ConnectOptions>
{
    private readonly ConnectOptions _connectOptions;
    
    public ConnectOptionsBuilder WithServer(string host, int port)
    {
        _connectOptions.Host = host;
        _connectOptions.Port = port;
        return this;
    }
    
    public ConnectOptionsBuilder WithClientId(string clientId)
    {
        _connectOptions.ClientId = clientId + "_" + Guid.NewGuid().ToString()[10..];
        return this;
    }
    
    public ConnectOptionsBuilder WithWill(string willTopic, string willMessage, QoS willQos, bool willRetain)
    {
        _connectOptions.WillTopic = willTopic;
        _connectOptions.WillMessage = willMessage;
        _connectOptions.WillQos = willQos;
        _connectOptions.WillRetain = willRetain;
        return this;
    }
    
    public ConnectOptionsBuilder WithCleanSession(bool cleanSession)
    {
        _connectOptions.CleanSession = cleanSession;
        return this;
    }
    
    public ConnectOptionsBuilder WithUserName(string userName)
    {
        _connectOptions.UserName = userName;
        return this;
    }
    
    public ConnectOptionsBuilder WithPassword(string password)
    {
        _connectOptions.Password = password;
        return this;
    }
    
    public ConnectOptions Build()
    {
        return _connectOptions;
    }

    public ConnectOptionsBuilder()
    {
        _connectOptions = new ConnectOptions();
    }
}