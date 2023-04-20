using MqttClient.ClientCore;
using MqttClient.ClientCore.Commands;
using MqttDataStructures;
using MqttDataStructures.Messages.Connects;

namespace MqttClient.ClientExtension.ClientCommands;

public class ClientConnectCommand : Command, IClientCommand
{
    public IMqttClient Client { get; }

    public override bool Execute(Message message, bool isStart)
    {
        return message switch
        {
            Connect connect => Connect(connect),
            ConnAck connAck => ConnAck(connAck),
            _ => throw new InvalidOperationException("Should never happen")
        };
    }

    private bool Connect(Connect connect)
    {
        Console.Out.WriteLine("Send Connect");
        Send(connect);
        return true;
    }

    private bool ConnAck(ConnAck connect)
    {
        if (Client.IsAuthorized())
        {
            return true;
        }
        Console.Out.WriteLine("Received ConnAck");
        Client.SetIsAuthorized(true);
        new Task(StartPinging).Start();
        return true;
    }
    
    private void StartPinging()
    {
        try
        {
            while (true)
            {
                Thread.Sleep(3000);
                if (!Client.IsAuthorized())
                {
                    return;
                }
                Client.PingRequest();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
    
    public ClientConnectCommand(IMqttClient client, TransmissionManager transmissionManager, MessageIdentifier? messageIdentifier)
        : base(transmissionManager, messageIdentifier)
    {
        Client = client;
    }
}