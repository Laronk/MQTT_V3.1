using Microsoft.CSharp.RuntimeBinder;
using MqttDataStructures;
using MqttDataStructures.Messages.Connects;
using MqttDataStructures.Messages.Pings;
using MqttDataStructures.Messages.Pubs;
using MqttDataStructures.Messages.Subs;
using MqttDataStructures.Messages.UnSubs;

namespace MessageConverter;

public partial class Converter
{
    //The message identifier is present in the variable header of the following MQTT messages: PUBLISH, PUBACK, PUBREC, PUBREL, PUBCOMP, SUBSCRIBE, SUBACK, UNSUBSCRIBE, UNSUBACK.
    //The payload is present in the following MQTT messages: PUBLISH, SUBSCRIBE, SUBACK, UNSUBSCRIBE, UNSUBACK.
    private static Message? Create(byte[] bytes, out bool dataCorrupted)
    {
        dataCorrupted = false;
        
        //it is fair to assume here that all required bytes are present
        if (bytes.Length < 2) return null;

        // TODO: FIX this!!!!! its only for debug porpose!!!
        FixedHeader fixedHeader;
        try
        {
            fixedHeader = new FixedHeader(new[] { bytes[0], bytes[1] });
        }
        catch (Exception e)
        {
            Console.WriteLine("BAD FIXED HEADER!!!!!!!!!!!!!!!!!!!!!");
            return null;
        }

        bytes = bytes.Skip(2).ToArray();
        if (bytes.Length < fixedHeader.RemainingLength) return null;

        try
        {
            return MakeMessageFromFixedHeader(fixedHeader, bytes);
        }
        catch (RuntimeBinderException e)
        {
            dataCorrupted = true;
            throw;
        }
        catch (Exception e)
        {
            Console.WriteLine("Could not create message from bytes: " + e.Message + " " + e.InnerException?.Message);
            dataCorrupted = bytes.Length >= fixedHeader.RemainingLength;
            return null;
        }
    }

    private static Message MakeMessageFromFixedHeader(FixedHeader fixedHeader, byte[] bytes)
    {
        return fixedHeader.MessageType switch
        {
            MessageType.Connect => new Connect(fixedHeader, bytes),
            MessageType.ConnAck => new ConnAck(fixedHeader, bytes),
            MessageType.Publish => new Publish(fixedHeader, bytes),
            MessageType.Subscribe => new Subscribe(fixedHeader, bytes),
            MessageType.SubAck => new SubAck(fixedHeader, bytes),
            MessageType.Unsubscribe => new Unsubscribe(fixedHeader, bytes),
            MessageType.UnsubAck => new UnsubAck(fixedHeader, bytes),
            MessageType.PingReq => new PingReq(fixedHeader, bytes),
            MessageType.PingResp => new PingResp(fixedHeader, bytes),
            MessageType.Disconnect => new Disconnect(fixedHeader, bytes),
            _ => throw new RuntimeBinderException("Should never happen! Server received wrong message: " + fixedHeader.MessageType)
        };
    }
}