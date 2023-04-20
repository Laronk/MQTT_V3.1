namespace MqttDataStructures.Building;

public interface IBuilder<out T> where T : IOptions
{
    public T Build();
}