using System.Globalization;
using System.Text.Json.Serialization;
using System.Text.Json;

public class DateOnlyJsonConverter : JsonConverter<DateOnly>
{
    private readonly string _format = "MM/dd/yyyy"; // Use MM/dd/yyyy format

    public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string dateString = reader.GetString();
        return DateOnly.ParseExact(dateString, _format, CultureInfo.InvariantCulture);
    }

    public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString(_format, CultureInfo.InvariantCulture));
    }
}
