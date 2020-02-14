using Microsoft.Extensions.Logging;

namespace BinanceDex.Utilities.Logging
{
    public sealed class FileLoggerProvider : ILoggerProvider
    {
        #region Fields

        private readonly ILogger fileLogger;

        #endregion

        #region Constructors

        public FileLoggerProvider(string filePath, LogLevel level)
        {
            this.fileLogger = new FileLogger(filePath, level);
        }

        #endregion

        #region Implements

        public void Dispose()
        {
        }

        public ILogger CreateLogger(string categoryName)
        {
            return this.fileLogger;
        }

        #endregion
    }
}