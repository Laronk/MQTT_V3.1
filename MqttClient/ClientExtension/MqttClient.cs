using System.Net.Sockets;
using MqttClient.ClientCore;
using MqttClient.ClientExtension.ClientCommands;
using MqttDataStructures;
using MqttDataStructures.Building.Options;
using MqttDataStructures.Messages.Connects;
using MqttDataStructures.Messages.Pings;
using MqttDataStructures.Messages.Pubs;
using MqttDataStructures.Messages.Subs;
using MqttDataStructures.Messages.UnSubs;

namespace MqttClient.ClientExtension;

// This interface exposes some advanced,
// server-specific functionality.
public class MqttClient : MqttClientCore, IMqttClient
{
    private string? _clientId;
    public Action<string, string>? OnPublishReceived;
    private bool _isAuthorized;

    public void Connect(ConnectOptions options)
    {
        if (IsAuthorized())
        {
            return;
        }

        try
        {
            _clientId = options.ClientId;
            TcpClient tcpClient = new TcpClient(options.Host, options.Port);
            SetTransmissionManager(tcpClient, new ClientCommandFactory(this));

            Message connect = new Connect(options);
            SendMade(connect);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    public void Publish(PublishOptions options)
    {
        if (IsAuthorized() is false)
        {
            return;
        }
        
        Publish publish = new Publish(options);
        SendMade(publish);
    }

    public void Subscribe(SubscribeOptions options)
    {
        if (IsAuthorized() is false)
        {
            return;
        }
        
        Subscribe subscribe = new Subscribe(options);
        SendMade(subscribe);
    }

    public void Unsubscribe(UnsubscribeOptions options)
    {
        if (IsAuthorized() is false)
        {
            return;
        }

        Unsubscribe unsubscribe = new Unsubscribe(options);
        SendMade(unsubscribe);
    }

    public void PingRequest()
    {
        if (IsAuthorized() is false)
        {
            return;
        }
        
        PingReq pingReq = new PingReq();
        SendMade(pingReq);
    }

    public void Disconnect()
    {
        if (IsAuthorized() is false)
        {
            return;
        }

        Disconnect disconnect = new Disconnect();
        SendMade(disconnect);
    }

    public void SetIsAuthorized(bool isAuthorized)
    {
        _isAuthorized = isAuthorized;
    }

    public bool IsAuthorized()
    {
        return _isAuthorized;
    }

    public void CloseTcp()
    {
        _clientId = null;
        CloseTransmissionManager();
    }
}