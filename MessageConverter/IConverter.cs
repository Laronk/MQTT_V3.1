using MqttDataStructures;

namespace MessageConverter;

public static partial class Converter
{
    /// <summary>
    /// Deserializes Message from bytes stream
    /// </summary>
    /// <param name="data">Bytes received from TCP stream</param>
    /// <param name="bytesConsumed">Says how many bytes was consumed to create returned Message.
    /// Notice that one TCP packet can have more bytes than necessary - bytes from next and/or incoming Message.
    /// 0 if couldn't create Message from given data. Message is null in this case.</param>
    /// <param name="dataCorrupted">True if there was an error non related to amount of bytes received from TCP</param>
    /// <returns>Message or null</returns>
    public static Message? ConvertToMessage(byte[] data, out int bytesConsumed, out bool dataCorrupted)
    {
        List<byte> copy = new List<byte>(data);
        if (Create(copy.ToArray(), out dataCorrupted) is not {} message)
        {
            bytesConsumed = 0;
            return null;
        }
        
        bytesConsumed = message.GetBytes().Count;
        return message;
    }
}