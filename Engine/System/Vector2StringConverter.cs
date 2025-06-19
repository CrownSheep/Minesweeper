using System;

namespace Minesweeper.System;

using Newtonsoft.Json;
using Microsoft.Xna.Framework;

public class Vector2StringConverter : JsonConverter<Vector2>
{
    public override void WriteJson(JsonWriter writer, Vector2 value, JsonSerializer serializer)
    {
        writer.WriteValue($"{value.X}, {value.Y}");
    }

    public override Vector2 ReadJson(JsonReader reader, Type objectType, Vector2 existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        string? str = reader.Value?.ToString();
        if (string.IsNullOrEmpty(str)) return Vector2.Zero;

        var parts = str.Split(',');
        if (parts.Length != 2) return Vector2.Zero;

        if (float.TryParse(parts[0], out float x) && float.TryParse(parts[1], out float y))
            return new Vector2(x, y);

        return Vector2.Zero;
    }
}
