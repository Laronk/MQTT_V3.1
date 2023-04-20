namespace MqttServer.SrvClientExtension;

public class WatchDog
{
    private readonly IMqttSrvClient _client;
    private const int Sleep = 3600;
    
    private void StartWatch()
    {
        while (true)
        {
            Thread.Sleep(Sleep);
            if (_client.LastSeen() < DateTime.UtcNow.AddDays(-1))
            {
                _client.RemoveClient();
            }
        }
    }
    
    public WatchDog(IMqttSrvClient client)
    {
        // TODO: implement this correctly!!!
        _client = client;
        new Task(StartWatch).Start();
    }
}