namespace MqttServer.SrvClientExtension.SrvCommands;

public interface ISrvCommand
{
    public IMqttSrvClient Client { get; }
}