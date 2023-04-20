using System.Text;
using MqttDataStructures.Building.Options;

namespace MqttDataStructures.Building.Builders;

public class PublishOptionsBuilder : IBuilder<PublishOptions>
{
    private readonly PublishOptions _publishOptions;

    public PublishOptionsBuilder WithQoS(QoS qoS)
    {
        _publishOptions.QoS = qoS;
        return this;
    }
    
    public PublishOptionsBuilder WithDup(bool dup)
    {
        _publishOptions.Dup = dup;
        return this;
    }
    
    public PublishOptionsBuilder WithRetain(bool retain)
    {
        _publishOptions.Retain = retain;
        return this;
    }
    
    public PublishOptionsBuilder WithTopicName(string topicName)
    {
        _publishOptions.TopicName = topicName;
        return this;
    }
    
    public PublishOptionsBuilder WithMessageIdentifier(ushort messageIdentifier)
    {
        _publishOptions.MessageIdentifier = messageIdentifier;
        return this;
    }
    
    public PublishOptionsBuilder WithPayload(string payload)
    {
        _publishOptions.Payload = Encoding.ASCII.GetBytes(payload); 
        //changed to ascii since this constructor is used for basic users, rather not willing to send weird characters
        //leaving this as UTF8 will break ToString() on Payload
        return this;
    }
    
    public PublishOptionsBuilder WithPayload(byte[] payload)
    {
        _publishOptions.Payload = payload;
        return this;
    }

    public PublishOptions Build()
    {
        return _publishOptions;
    }

    public PublishOptionsBuilder()
    {
        _publishOptions = new PublishOptions();
    }
}