namespace MqttDataStructures.Messages.Connects;

public class ConnAckVarHead : IVariableHeader
{
    public MessageIdentifier? MessageIdentifier
    {
        get => null;
        set => throw new InvalidOperationException("Should never happen");
    }

    public ConnectReturnCode ReturnCode { get; }

    public List<byte> GetBytes() => new() {(byte) 0, (byte)ReturnCode};

    public ConnAckVarHead(byte[] bytes)
    {
        ReturnCode = (ConnectReturnCode)bytes[1];
    }
    
    public ConnAckVarHead(ConnectReturnCode returnCode)
    {
        ReturnCode = returnCode;
    }
}