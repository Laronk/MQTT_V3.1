using MqttDataStructures.Utils;

namespace MqttDataStructures.Messages.Connects;

public class ConnectVarHead : IVariableHeader
{
    public MessageIdentifier? MessageIdentifier
    {
        get => null;
        set => throw new InvalidOperationException("Should never happen");
    }

    public string ProtocolName { get; }
    public ushort ProtocolVersionNumber { get; }
    public ConnectFlags ConnectFlags { get; }
    public uint KeepAliveTimer { get; }

    public List<byte> GetBytes()
    {
        var bytes = new List<byte>();

        bytes.AddRange(BytesOperator.StringToBytes(ProtocolName));
        bytes.Add((byte)(ProtocolVersionNumber));
        bytes.Add(ConnectFlags.AsByte());
        bytes.Add((byte)(KeepAliveTimer >> 8));
        bytes.Add((byte)(KeepAliveTimer & 0xFF));

        return bytes;
    }

    public ConnectVarHead(byte[] data, out byte[] remainingData)
    {
        ProtocolName = BytesOperator.GetStringFromStream(data, out data);
        ProtocolVersionNumber = data[0];
        ConnectFlags = new ConnectFlags(data[1]);
        KeepAliveTimer = (uint)((data[2] << 8) + data[3]);
        remainingData = data.Skip(4).ToArray();
    }

    public ConnectVarHead(string protocolName, ushort protocolVersionNumber, ConnectFlags connectFlags,
        ushort keepAliveTimer)
    {
        ProtocolName = protocolName;
        ProtocolVersionNumber = protocolVersionNumber;
        ConnectFlags = connectFlags;
        KeepAliveTimer = keepAliveTimer;
    }
}