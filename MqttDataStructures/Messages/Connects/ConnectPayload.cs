using MqttDataStructures.Building.Options;
using MqttDataStructures.Utils;

namespace MqttDataStructures.Messages.Connects;

public class ConnectPayload : IPayload
{
    public string ClientId { get; }
    public string? WillTopic { get; }
    public string? WillMessage { get; }
    public string? Username { get; }
    public string? Password { get; }

    public List<byte> GetBytes()
    {
        List<byte> bytes = new List<byte>();
        bytes.AddRange(BytesOperator.StringToBytes(ClientId));
        bytes.AddRange(BytesOperator.StringToBytes(WillTopic));
        bytes.AddRange(BytesOperator.StringToBytes(WillMessage));
        bytes.AddRange(BytesOperator.StringToBytes(Username));
        bytes.AddRange(BytesOperator.StringToBytes(Password));
        return bytes;
    }

    public ConnectPayload(byte[] data, ConnectVarHead varHead)
    {
        ClientId = BytesOperator.GetStringFromStream(data, out data);
        if (varHead.ConnectFlags.WillFlag)
        {
            WillTopic = BytesOperator.GetStringFromStream(data, out data);
            WillMessage = BytesOperator.GetStringFromStream(data, out data);
        }
        if (varHead.ConnectFlags.UsernameFlag) Username = BytesOperator.GetStringFromStream(data, out data);
        if (varHead.ConnectFlags.PasswordFlag) Password = BytesOperator.GetStringFromStream(data, out data);
    }

    public ConnectPayload(ConnectOptions options)
    {
        ClientId = options.ClientId;
        Username = options.UserName;
        Password = options.Password;
        WillTopic = options.WillTopic;
        WillMessage = options.WillMessage;
    }
}