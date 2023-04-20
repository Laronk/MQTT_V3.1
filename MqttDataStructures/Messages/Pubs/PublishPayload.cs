using System.Text;

namespace MqttDataStructures.Messages.Pubs;

public class PublishPayload : IPayload
{
    /// <summary>
    /// You can use get property, however, concider using GetString() method instead.
    /// </summary>
    private byte[] Payload { get; }

    /// <summary>
    /// Use this to construct TCP message
    /// </summary>
    /// <returns></returns>
    public List<byte> GetBytes()
    {
        return new List<byte>(Payload);
    }

    public string GetString() { return Encoding.ASCII.GetString(Payload); }
    
    /// <summary>
    /// This is to make it easier to read the content in the debuger
    /// Do not use it to convert the payload to string, use GetString instead
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return String.Join("", Payload.ToList().SelectMany(c =>
        {
            string result = string.Empty;
            int i = (int)c;
            //check if c is in ascii range
            if (i >= 32 && i <= 126) { result += (char)i; } else { result += $"[{i}]"; }
            return result;
        }));
        //return Encoding.UTF8.GetString(Payload); 

        //Example: [2,1,3,7,50,51,32,97,108,97] will be converted to:
        // old formula:  ����23 ala
        // new formula:  [2][1][3][7]23 ala
    }
    //Here is part of ascii table:
    // 0 - 48, 1 - 49, 2 - 50, 3 - 51, 4 - 52, 5 - 53, 6 - 54, 7 - 55, 8 - 56, 9 - 57,
    // A - 65, B - 66, C - 67, D - 68, E - 69, F - 70, G - 71, H - 72, I - 73, J - 74, K - 75, L - 76, M - 77, N - 78, O - 79, P - 80, Q - 81, R - 82, S - 83, T - 84, U - 85, V - 86, W - 87, X - 88, Y - 89, Z - 90,
    // a - 97, b - 98, c - 99, d - 100, e - 101, f - 102, g - 103, h - 104, i - 105, j - 106, k - 107, l - 108, m - 109, n - 110, o - 111, p - 112, q - 113, r - 114, s - 115, t - 116, u - 117, v - 118, w - 119, x - 120, y - 121, z - 122,


    /// <summary>
    /// Use this on server when receiving message from client
    /// and to create new message to send to other clients
    /// </summary>
    /// <param name="bytes"></param>
    public PublishPayload(byte[] bytes) { Payload = bytes; }
}