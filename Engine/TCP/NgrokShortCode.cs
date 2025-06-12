using System;
using System.Text;

namespace Minesweeper;

public static class NgrokShortCode
{
    const string BASE36 = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    
    public static string Encode(string address)
    {
        // Parse "X.tcp.eu.ngrok.io:PORT"
        var parts = address.Split(new[] { '.', ':' }, StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length < 5 || !int.TryParse(parts[0], out int subdomain) || !int.TryParse(parts[4], out int port))
            throw new FormatException("Expected format: X.tcp.eu.ngrok.io:PORT");

        // Encode into a single number
        int combined = (subdomain * 1000000) + port; // 1 digit for X, up to 6 digits for port

        // Convert to base36
        return ToBase36(combined).PadLeft(6, '0'); // ensure 6-character code
    }

    public static string Decode(string code)
    {
        int combined = FromBase36(code);

        int subdomain = combined / 1000000;
        int port = combined % 1000000;

        return $"{subdomain}.tcp.eu.ngrok.io:{port}";
    }

    private static string ToBase36(int value)
    {
        var sb = new StringBuilder();
        while (value > 0)
        {
            sb.Insert(0, BASE36[value % 36]);
            value /= 36;
        }
        return sb.ToString();
    }

    private static int FromBase36(string input)
    {
        int result = 0;
        foreach (char c in input.ToUpper())
        {
            result = result * 36 + BASE36.IndexOf(c);
        }
        return result;
    }
}
