namespace MqttDataStructures.Utils;

public static class BytesOperator
{
    public static string GetStringFromStream(byte[] stream, out byte[] remainingBytes)
    {
        string? result = GetStringFromStream(stream, out int consumedBytes);
        remainingBytes = Shift(stream, consumedBytes);
        
        return result;
    }

    public static List<byte> StringToBytes(string? str)
    {
        if (str == null)
        {
            return new List<byte>();
        }

        List<byte> result = new List<byte>();
        result.Add((byte)(str.Length / 256));
        result.Add((byte)(str.Length % 256));
        foreach (char c in str) result.Add((byte)c);
        
        return result;
    }
    
    private static byte[] Shift(byte[] original, int shift)
    {
        if (original.Length < shift)
        {
            throw new ArgumentException("Error: Could not shift array. Shift is greater than array length.");
        }

        byte[] shifted = new byte[original.Length - shift];
        Buffer.BlockCopy(original, shift, shifted, 0, shifted.Length);
        
        return shifted;
    }
    
    private static string GetStringFromStream(byte[] stream, out int consumedBytes)
    {
        string result = "";
        
        if (stream.Length < 2)
        {
            throw new ArgumentException("Error: Could not read string from stream. Less than 2 bytes available.");
            consumedBytes = 0;
            
            return result;
        }

        int len = stream[0] * 256 + stream[1];
        consumedBytes = 2 + len;
        if (stream.Length < consumedBytes)
        {
            throw new ArgumentException("Error: Could not read string from stream. Not enough bytes. Bytes available: "
                                        + stream.Length + ", Bytes needed: " + consumedBytes);
            consumedBytes = 0;
            
            return null;
        }

        for (int i = 0; i < len; i++) result += (char)stream[i + 2];
        return result;
    }
}