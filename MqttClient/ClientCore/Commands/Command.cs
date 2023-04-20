using MqttDataStructures;
using MqttDataStructures.Messages;

namespace MqttClient.ClientCore.Commands;

public abstract class Command
{
    private readonly TransmissionManager _transmissionManager;
    private readonly MessageIdentifier? _messageIdentifier;

    protected void Send(Message message)
    {
        _transmissionManager.Send(message);
    }

    public bool HasMessageIdentifier()
    {
        return _messageIdentifier is not null;
    }

    public bool HasMatchingMessageIdentifier(ushort messageIdentifier)
    {
        if (_messageIdentifier is null)
        {
            return false;
        }
        
        return _messageIdentifier.Value == messageIdentifier;
    }

    public ushort? GetMessageIdentifier()
    {
        if (_messageIdentifier is null)
        {
            return null;
        }
        
        return _messageIdentifier.Value;
    }

    public abstract bool Execute(Message message, bool isStart);

    protected Command(TransmissionManager transmissionManager, MessageIdentifier? messageIdentifier)
    {
        _transmissionManager = transmissionManager;
        _messageIdentifier = messageIdentifier;
    }
}