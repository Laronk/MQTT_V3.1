namespace MqttDataStructures.Messages.Subs;

public class SubAckPayload : IPayload
{
    private readonly List<QoS> _grantedQosLevels;
    public List<QoS> GrantedQoSLevels => new(_grantedQosLevels);

    public List<byte> GetBytes() =>
        GrantedQoSLevels.Select(x => (byte)x).ToList();

    public SubAckPayload(byte[] bytes)
    {
        _grantedQosLevels = bytes.ToList().Select(x => (QoS)x).ToList();
    }
    
    public SubAckPayload(List<QoS> grantedQosLevels)
    {
        _grantedQosLevels = new List<QoS>(grantedQosLevels);
    }
}