using Microsoft.Extensions.Logging;

namespace Application.Services;

public class LogService
{
    private readonly ILogger<LogService> _logger;

    public LogService(ILogger<LogService> logger)
    {
        _logger = logger;
    }
}
