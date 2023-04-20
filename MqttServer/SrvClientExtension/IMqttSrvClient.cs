using MqttDataStructures.Messages.Pubs;
using MqttDataStructures.Messages.Subs;
using MqttDataStructures.Messages.UnSubs;

namespace MqttServer.SrvClientExtension;

public interface IMqttSrvClient
{
    public void MakePublishToAllSubscribed(Publish publish);
    public bool NewSubscription(Subscribe subscribe);
    public bool RemoveSubscription(Unsubscribe unsubscribe);
    public void RemoveClient();
    public DateTime LastSeen();
    public string? ClientId();
    public bool TrySetClientId(string clientId);
    public void ProcedureLastWill();
    public void SaveLastWill(Publish publish);
    public bool CheckUserName(string? userName);
    public bool CheckUserCredentials(string? userName, string? userPassword);
}