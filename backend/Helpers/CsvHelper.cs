using System.Text;

namespace CgbSteps.Api.Helpers;

/// <summary>Minimal CSV reading: comma separated, optional double quotes, one record per line.</summary>
public static class CsvHelper
{
    public static async Task<List<string[]>> ReadAllAsync(Stream stream, CancellationToken cancellationToken)
    {
        using var reader = new StreamReader(stream, Encoding.UTF8, detectEncodingFromByteOrderMarks: true);
        var rows = new List<string[]>();

        while (await reader.ReadLineAsync(cancellationToken) is { } line)
        {
            rows.Add(ParseLine(line));
        }

        return rows;
    }

    public static string[] ParseLine(string line)
    {
        var fields = new List<string>();
        var current = new StringBuilder();
        var inQuotes = false;

        for (var i = 0; i < line.Length; i++)
        {
            var c = line[i];

            if (inQuotes && c == '"' && i + 1 < line.Length && line[i + 1] == '"')
            {
                current.Append('"');
                i++;
            }
            else if (c == '"')
            {
                inQuotes = !inQuotes;
            }
            else if (c == ',' && !inQuotes)
            {
                fields.Add(current.ToString().Trim());
                current.Clear();
            }
            else
            {
                current.Append(c);
            }
        }

        fields.Add(current.ToString().Trim());
        return [.. fields];
    }
}
