using MqttDataStructures.Messages;

namespace MqttDataStructures;

public interface IPayload : IGetBytes
{
    public int BytesConsumed => GetBytes().Count;
}