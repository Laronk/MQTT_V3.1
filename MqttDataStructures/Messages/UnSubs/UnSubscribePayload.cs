using MqttDataStructures.Utils;

namespace MqttDataStructures.Messages.UnSubs;

public class UnSubscribePayload : IPayload
{
    private readonly List<string> _topics;

    public List<byte> GetBytes() => _topics
        .SelectMany(BytesOperator.StringToBytes)
        .ToList();

    public List<string> Topics => new(_topics);

    public UnSubscribePayload(byte[] bytes)
    {
        _topics = new List<string>();
        while (bytes.Length > 0)
        {
            _topics.Add(BytesOperator.GetStringFromStream(bytes, out bytes));
        }
    }

    public UnSubscribePayload(List<string> topicFilters)
    {
        _topics = new List<string>(topicFilters);
    }
}