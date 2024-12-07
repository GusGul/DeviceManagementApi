using System.Text.Json.Serialization;

namespace Domain.DTOs;

public class DevicePatchDTO
{
    [JsonIgnore]
    public int? Id { get; set; }
    public string? Name { get; set; }
    public string? Brand { get; set; }
}