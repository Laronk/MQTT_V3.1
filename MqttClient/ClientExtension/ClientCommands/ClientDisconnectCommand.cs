using MqttClient.ClientCore;
using MqttClient.ClientCore.Commands;
using MqttDataStructures;
using MqttDataStructures.Messages.Connects;

namespace MqttClient.ClientExtension.ClientCommands;

public class ClientDisconnectCommand : Command, IClientCommand
{
    public IMqttClient Client { get; }

    public override bool Execute(Message message, bool isStart)
    {
        if (message is not Disconnect disconnect) return false;
        
        lock (Client)
        {
            Client.SetIsAuthorized(false);
        }
            
        Console.Out.WriteLine("Send Disconnect");
        Thread.Sleep(1000);
        Send(disconnect);
        Client.CloseTcp();
        return true;
    }

    public ClientDisconnectCommand(IMqttClient client, TransmissionManager transmissionManager, MessageIdentifier? messageIdentifier)
        : base(transmissionManager, messageIdentifier)
    {
        Client = client;
    }
}