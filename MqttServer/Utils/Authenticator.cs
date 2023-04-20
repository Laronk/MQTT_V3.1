namespace MqttServer.Utils;

public class Authenticator
{
    private readonly List<Account> _accounts;

    public bool CheckCredentials(string? userName, string? userPassword)
    {
        if (_accounts.Count == 0)
        {
            return true;
        }
        
        if (userName is null or "" || userPassword == "")
        {
            return false;
        }
        
        return _accounts.Any(ac => ac.UserName  == userName && ac.PassMatch(userPassword));
    }

    public bool IsUserNameValid(string? userName)
    {
        if (_accounts.Count == 0)
        {
            return true;
        }
        
        if (userName is null or "")
        {
            return false;
        }
        
        return _accounts.Any(ac => ac.UserName  == userName);
    }
    
    public Authenticator(List<Account> accounts)
    {
        _accounts = new List<Account>(accounts);
    }
}