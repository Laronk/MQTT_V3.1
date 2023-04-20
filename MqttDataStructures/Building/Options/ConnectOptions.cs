namespace MqttDataStructures.Building.Options;

public sealed class ConnectOptions : IOptions
{
    public string Host { get; set; }
    public int Port { get; set; }
    public string ClientId { get; set; }
    public string? WillTopic { get; set; }
    public string? WillMessage { get; set; }
    public QoS WillQos { get; set; }
    public bool WillRetain { get; set; }
    public bool CleanSession { get; set; }
    public string? UserName { get; set; }
    public string? Password { get; set; }

    public ConnectOptions()
    {
        Host = "";
        Port = 0;
        ClientId = "";
        WillTopic = null;
        WillMessage = null;
        WillQos = QoS.FireAndForget;
        WillRetain = false;
        CleanSession = true;
        UserName = null;
        Password = null;
    }
}