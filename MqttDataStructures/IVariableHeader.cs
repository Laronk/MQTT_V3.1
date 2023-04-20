using MqttDataStructures.Messages;

namespace MqttDataStructures;

public interface IVariableHeader : IGetBytes
{
    public int BytesConsumed => GetBytes().Count;

    public MessageIdentifier? MessageIdentifier { get; set; }
}