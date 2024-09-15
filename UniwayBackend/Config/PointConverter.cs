using NetTopologySuite.Geometries;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace UniwayBackend.Config
{
    /// <summary>
    /// Configuración para poder serializar las propiedades de tipo NetTopologySuite.Geometries.Point
    /// </summary>
    public class PointConverter : JsonConverter<Point>
    {
        public override Point? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // Si el valor es nulo, devuelve null
            if (reader.TokenType == JsonTokenType.Null) return null;

            double? longitude = null;
            double? latitude = null;

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    break;
                }

                if (reader.TokenType == JsonTokenType.PropertyName)
                {
                    string propertyName = reader.GetString();

                    reader.Read(); // Moverse al valor

                    if (propertyName == "lng")
                    {
                        longitude = reader.GetDouble();
                    }
                    else if (propertyName == "lat")
                    {
                        latitude = reader.GetDouble();
                    }
                }
            }

            if (!longitude.HasValue && !latitude.HasValue)
                throw new JsonException("El JSON no contiene valores válidos para 'lng' y 'lat'.");
            
            return new Point(longitude.Value, latitude.Value) { SRID = 4326 };
        }

        public override void Write(Utf8JsonWriter writer, Point value, JsonSerializerOptions options)
        {
            if (value == null)
            {
                writer.WriteNullValue(); // Si el valor es nulo, escribe null en el JSON
                return;
            }

            writer.WriteStartObject();
            writer.WritePropertyName("lng");
            writer.WriteNumberValue(value.X);
            writer.WritePropertyName("lat");
            writer.WriteNumberValue(value.Y);
            writer.WriteEndObject();
        }
    }
}
