using Application.Services;
using Microsoft.Extensions.Logging;
using Moq;

public class LogServiceTests
{
    private readonly Mock<ILogger<LogService>> _mockLogger;
    private readonly LogService _logService;

    public LogServiceTests()
    {
        _mockLogger = new Mock<ILogger<LogService>>();
        
        _logService = new LogService(_mockLogger.Object);
    }

    [Fact]
    public void LogMessage_Should_Call_LogInformation()
    {
        var message = "Test log message";

        _logService.LogMessage(message);

        _mockLogger.Verify(logger => logger.Log(
                LogLevel.Information, 
                It.IsAny<EventId>(), 
                It.Is<It.IsAnyType>((state, t) => state.ToString() == message), 
                It.IsAny<Exception>(), 
                It.Is<Func<It.IsAnyType, Exception, string>>((formatter, exception) => true)),
            Times.Once);
    }
}