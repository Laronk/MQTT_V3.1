namespace MqttDataStructures.Messages.Connects;

public enum ConnectReturnCode
{
    /*0	0x00	Connection Accepted
    1	0x01	Connection Refused: unacceptable protocol version
    2	0x02	Connection Refused: identifier rejected
    3	0x03	Connection Refused: server unavailable
    4	0x04	Connection Refused: bad user name or password
    5	0x05	Connection Refused: not authorized
    6-255		Reserved for future use
    */
    ConnectionAccepted = 0,
    ConnectionRefusedUnacceptableProtocolVersion = 1,
    ConnectionRefusedIdentifierRejected = 2,
    ConnectionRefusedServerUnavailable = 3,
    ConnectionRefusedBadUserNameOrPassword = 4,
    ConnectionRefusedNotAuthorized = 5,
}