namespace MqttDataStructures;

public abstract class Message : IGetBytes
{
    public FixedHeader FixedHeader { get; }
    public abstract MessageIdentifier? GetMessageIdentifier();

    public abstract void SetMessageIdentifier(ushort newMessageIdentifier);

    protected abstract void SetRemainingLength();
    
    public abstract List<byte> GetBytes();

    public override string ToString()
    {
        List<byte> result = GetBytes().ToList();
        return string.Join(", ", result.Select(x => ((int)x).ToString()));
    }
    
    protected Message(FixedHeader fixedHeader)
    {
        FixedHeader = fixedHeader;
    }
}