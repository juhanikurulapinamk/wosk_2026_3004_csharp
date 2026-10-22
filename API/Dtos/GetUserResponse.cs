using System.Text.Json.Serialization;

namespace Api.Dtos;

public class GetUserResponse
{
    public long? Id { get; set; } = null;

    // JsonPropertyName pitää vastauksen kenttien nimet samoina kuin
    // ensimmäisessä versiossa (first_name, last_name)
    [JsonPropertyName("first_name")]
    public required string Firstname { get; set; }

    [JsonPropertyName("last_name")]
    public required string Lastname { get; set; }

    public required string Username { get; set; }
}