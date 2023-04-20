using System.Net.Sockets;
using MessageConverter;
using MqttClient.ClientCore.Commands;
using MqttDataStructures;
using MqttDataStructures.Messages.Connects;

namespace MqttClient.ClientCore;

public class TransmissionManager
{
    private readonly TcpClient _tcpClient;
    private readonly List<Command> _commands;
    private readonly ICommandFactory _commandFactory;

    private readonly NetworkStream _netStream;
    private readonly byte[] _buffer;
    private readonly List<byte> _capturedBytes;

    public void CloseConnection()
    {
        _tcpClient.Close();
    }

    private void StartListening(IAsyncResult iar)
    {
        try
        {
            _netStream.EndRead(iar);
            _capturedBytes.Add(_buffer[0]);

            OnTcpMessage();

            _netStream.BeginRead(_buffer, 0, 1, StartListening, null);
        }
        catch (Exception e)
        {
            Console.WriteLine("Lost connection: " + e.Message);
            Message disconnect = new Disconnect();
            _commandFactory.CreateCommand(disconnect, this).Execute(disconnect, true);
        }
    }

    private Command GetNewOrExistingCommand(Message message)
    {
        Command command = _commandFactory.CreateCommand(message, this);

        if (command.HasMessageIdentifier() is false)
        {
            return command;
        }

        if (command.GetMessageIdentifier() is not { } messageIdentifier)
        {
            return command;
        }

        if (_commands.SingleOrDefault(com => com.HasMatchingMessageIdentifier(messageIdentifier)) is { } existingCommand)
        {
            return existingCommand;
        }

        return command;
    }

    private Command GetNewCommand(Message message)
    {
        Command command = _commandFactory.CreateCommand(message, this);
        
        if (command.GetMessageIdentifier() is not { } messageIdentifier)
        {
            return command;
        }

        lock (_commands)
        {
            var messageIds = _commands
                .Where(com => com.HasMessageIdentifier())
                .Select(com => com.GetMessageIdentifier())
                .ToList();

            var newId = (ushort)new Random().Next(1, 65536);
            while (messageIds.Contains(newId))
            {
                newId = (ushort)new Random().Next(1, 65536);
            }

            if (message.GetMessageIdentifier() != null)
            // if (message.GetMessageIdentifier() != null && messageIds.Contains(message.GetMessageIdentifier()!.Value))
            {
                message.SetMessageIdentifier(newId);
            }

            command = _commandFactory.CreateCommand(message, this);
            _commands.Add(command);
            return command;
        }
    }

    public void Send(Message message)
    {
        try
        {
            byte[] messageBytes = message.GetBytes().ToArray();
            _netStream.Write(messageBytes, 0, messageBytes.Length);
        }
        catch (Exception e)
        {
            Console.WriteLine("Exception in Send: " + e.Message);
        }
    }

    public void SendMade(Message message)
    {
        Command command;
        if (message.GetMessageIdentifier() is null)
        {
            command = _commandFactory.CreateCommand(message, this);
            command.Execute(message, true);
            return;
        }

        command = GetNewCommand(message);
        if (command.Execute(message, true))
        {
            _commands.Remove(command);
        }
    }

    private void OnTcpMessage()
    {
        if (Converter.ConvertToMessage(
                _capturedBytes.ToArray(),
                out int bytesConsumed,
                out bool dataCorrupted) is not { } message)
        {
            if (dataCorrupted)
            {
                _capturedBytes.Clear();
            }

            return;
        }

        _capturedBytes.RemoveRange(0, bytesConsumed);

        Command command = GetNewOrExistingCommand(message);
        if (command.Execute(message, false))
        {
            _commands.Remove(command);
        }
    }

    public TransmissionManager(TcpClient tcpClient, ICommandFactory commandFactory)
    {
        _commandFactory = commandFactory;
        _tcpClient = tcpClient;
        _commands = new List<Command>();

        _buffer = new byte[1];
        _capturedBytes = new List<byte>();

        _netStream = _tcpClient.GetStream();
        _netStream.BeginRead(_buffer, 0, 1, StartListening, null);
    }
}