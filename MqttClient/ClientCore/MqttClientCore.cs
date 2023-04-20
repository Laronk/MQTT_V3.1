using System.Net.Sockets;
using MqttClient.ClientCore.Commands;
using MqttDataStructures;

namespace MqttClient.ClientCore;

public abstract class MqttClientCore
{
    private TransmissionManager? _transmissionManager;

    protected void CloseTransmissionManager()
    {
        _transmissionManager?.CloseConnection();
    }
    
    protected void SetTransmissionManager(TcpClient tcpClient, ICommandFactory commandFactory)
    {
        _transmissionManager?.CloseConnection();
        _transmissionManager = new TransmissionManager(tcpClient, commandFactory);
    }

    protected void Send(Message message)
    {
        _transmissionManager?.Send(message);
    }
    
    protected void SendMade(Message message)
    {
        _transmissionManager?.SendMade(message);
    }
}