
namespace Logging.Services
{
    public class FileLoggerProvider : ILoggerProvider
    {
        private readonly IHostEnvironment _env;
        public FileLoggerProvider(IHostEnvironment env)
        {
            _env = env;
        }
        public ILogger CreateLogger(string categoryName)
        {
            return new FileLogger(_env);
        }

        public void Dispose()
        {
        }
    }

    public class FileLogger : ILogger
    {
        private readonly IHostEnvironment _env;
        public FileLogger(IHostEnvironment env)
        {
            _env = env;
        }

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel != LogLevel.None;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            if(!IsEnabled(logLevel))
            {
                return;
            }

            var now = DateTime.UtcNow;
            var stateResult = exception != null ? exception.StackTrace : "";
            var logRecord = $"[{now:yyyy-MM-dd HH:mm:ss+00:00}] [{logLevel}] {formatter(state, exception)} {stateResult}";

            var directoryPath = Path.Combine(_env.ContentRootPath, "Logs");
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            var fileName = $"{now:yyyyMMdd}_Logs.txt";
            var filePath = Path.Combine(directoryPath, fileName);

            using var streamWriter = new StreamWriter(filePath, true);
            streamWriter.WriteLine(logRecord);
            streamWriter.Close();

        }
    }
}
