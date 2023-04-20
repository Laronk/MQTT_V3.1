namespace MqttServer.Utils;

public class Account
{
    public string UserName { get; }
    private readonly PassHash? _passHash;
    
    public bool PassMatch(string? userPassword)
    {
        if (_passHash is null)
        {
            return userPassword is null;
        }

        return _passHash.Match(userPassword);
    }
    
    public Account(string userName)
    {
        UserName = userName;
        _passHash = null;
    }
    
    public Account(string userName, PassHash passHash)
    {
        UserName = userName;
        _passHash = passHash;
    }
}