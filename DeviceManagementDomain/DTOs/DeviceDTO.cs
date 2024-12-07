using System.Text.Json.Serialization;

namespace Domain.DTOs;

public class DeviceDTO
{
    [JsonIgnore]
    public int? Id { get; set; }
    public string Name { get; set; }
    public string Brand { get; set; }
}