using MqttDataStructures;

namespace MqttClient.ClientCore.Commands;

public interface ICommandFactory
{
    public Command CreateCommand(Message m, TransmissionManager transmissionManager);
}