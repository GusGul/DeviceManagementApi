using System.Text.Json.Serialization;

namespace Application.DTOs;

public class DevicePatchDTO
{
    [JsonIgnore]
    public int? Id { get; set; }
    public string? Name { get; set; }
    public string? Brand { get; set; }
}