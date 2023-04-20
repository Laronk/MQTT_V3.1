namespace MqttClient.ClientExtension.ClientCommands;

public interface IClientCommand
{
    public IMqttClient Client { get; }
}