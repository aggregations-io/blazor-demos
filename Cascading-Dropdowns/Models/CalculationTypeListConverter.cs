using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cascading_Dropdowns.Models;

public class CalculationTypeListConverter : JsonConverter<List<CalculationType>>
{
    public override List<CalculationType> Read(ref Utf8JsonReader reader, Type typeToConvert,
        JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartArray)
        {
            throw new Exception("Not an array");
        }

        var ret = new List<CalculationType>();

        while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                var this_str = reader.GetString();
                if (Enum.TryParse<CalculationType>(this_str, out CalculationType _calculation))
                {
                    ret.Add(_calculation);
                }
                else
                {
                    throw new Exception($"Cannot parse {this_str} as CalculationType");
                }
            }
            else
            {
                throw new Exception("Cannot Parse non string");
            }
        }

        return ret;
    }

    public override void Write(Utf8JsonWriter writer, List<CalculationType> value, JsonSerializerOptions options)
    {
        writer.WriteStartArray();
        foreach (var v in value)
        {
            writer.WriteStringValue(v.ToString());
        }

        writer.WriteEndArray();
    }
}