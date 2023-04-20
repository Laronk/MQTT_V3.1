using System.Net.Sockets;
using MqttClient.ClientCore;
using MqttDataStructures.Messages.Pubs;
using MqttDataStructures.Messages.Subs;
using MqttDataStructures.Messages.UnSubs;
using MqttServer.Publisher;
using MqttServer.SrvClientExtension.SrvCommands;

namespace MqttServer.SrvClientExtension;

public class MqttSrvClient : MqttClientCore, IMqttSrvClient, ISubscriber
{
    private readonly IBroker _broker;
    private readonly Action<IMqttSrvClient> _removeClient;
    private readonly WatchDog _watchDog;
    private DateTime _lastSeen;
    private string? _clientId;
    private Publish? _lastWill;

    public string? ClientId()
    {
        return _clientId;
    }

    public string GetSubscriberId()
    {
        if (_clientId is null)
        {
            throw new InvalidOperationException("Should never happen");
        }
        
        return _clientId;
    }

    public bool TrySetClientId(string clientId)
    {
        if (!_broker.IsClientIdValid(clientId))
        {
            return false;
        }
        
        _clientId = clientId;
        return true;
    }

    public bool CheckUserName(string? userName)
    {
        return _broker.IsUserNameValid(userName);
    }

    public bool CheckUserCredentials(string? userName, string? userPassword)
    {
        return _broker.CheckCredentials(userName, userPassword);
    }
    
    public void SaveLastWill(Publish publish)
    {
        _lastWill = publish;
    }

    public void ProcedureLastWill()
    {
        if (_lastWill is not null)
        {
            _broker.MakePublishToAllSubscribed(_lastWill);
        }
    }

    public void MakePublishToAllSubscribed(Publish publish)
    {
        _broker.MakePublishToAllSubscribed(publish);
    }

    public void GotPublish(Publish publish)
    {
        SendMade(publish);
    }

    public bool NewSubscription(Subscribe subscribe)
    {
        UpdateLastSeen();
        foreach (var filter in subscribe.Payload.Subscriptions)
        {
            _broker.NewSubscription(
                this,
                filter
            );
        }

        return true;
    }

    public bool RemoveSubscription(Unsubscribe unsubscribe)
    {
        UpdateLastSeen();
        foreach (var filter in unsubscribe.Payload.Topics)
        {
            _broker.RemoveSubscription(
                this,
                filter
            );
        }

        return true;
    }

    public void RemoveClient()
    {
        CloseTransmissionManager();
        
        if (_clientId is null) return;
        
        _removeClient?.Invoke(this);
        _clientId = null;
    }

    public DateTime LastSeen()
    {
        return _lastSeen;
    }

    private void UpdateLastSeen()
    {
        _lastSeen = DateTime.UtcNow;
    }

    public MqttSrvClient(TcpClient tcpClient, IBroker broker, Action<IMqttSrvClient> removeClient)
    {
        UpdateLastSeen();
        _broker = broker;
        _removeClient = removeClient;
        _watchDog = new WatchDog(this);
        _lastWill = null;
        SetTransmissionManager(tcpClient, new SrvCommandFactory(this));
    }
}