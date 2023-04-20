namespace MqttServer.Utils;

public class PassHash
{
    private const int Salt = 12;
    private readonly string _password;

    public PassHash(string passwordToHash)
    {
        _password = BCrypt.Net.BCrypt.HashPassword(passwordToHash, Salt);
    }

    public bool Match(string? userPassword)
    {
        return userPassword is not null && BCrypt.Net.BCrypt.Verify(userPassword, _password);
    }
}