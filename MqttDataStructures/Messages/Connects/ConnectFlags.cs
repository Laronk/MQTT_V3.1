namespace MqttDataStructures.Messages.Connects;

public class ConnectFlags
{
    private byte Flags {get; }
    public byte AsByte() => Flags;
    
    public bool UsernameFlag { get; }
    public bool PasswordFlag { get; }
    public bool CleanSession { get; }
    public bool WillFlag { get; }
    public QoS? WillQos { get; }
    public bool WillRetain { get; }

    public ConnectFlags(byte flags)
    {
        Flags = flags;
        UsernameFlag = (flags & 128) == 128;
        PasswordFlag = (flags & 64) == 64;
        WillRetain = (flags & 32) == 32;
        WillQos = (QoS)((flags & 24) >> 3);
        WillFlag = (flags & 4) == 4;
        CleanSession = (flags & 2) == 2;
    }

    public ConnectFlags(
        bool usernameFlag, 
        bool passwordFlag,
        bool cleanSession, 
        bool willFlag, QoS willQos, bool willRetain)
    {
        UsernameFlag = usernameFlag;
        PasswordFlag = passwordFlag;
        CleanSession = cleanSession;
        WillFlag = willFlag;
        WillQos = willFlag is false ? null : willQos;
        WillRetain = willRetain;
        
        Flags = 0;
        if (usernameFlag) Flags |= 128;
        if (passwordFlag) Flags |= 64;
        if (willRetain) Flags |= 32;
        Flags |= (byte)((byte)willQos << 3);
        if (willFlag) Flags |= 4;
        if (cleanSession) Flags |= 2;
    }
}