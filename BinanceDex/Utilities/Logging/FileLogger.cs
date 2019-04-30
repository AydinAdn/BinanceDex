using System;
using System.IO;
using Microsoft.Extensions.Logging;

namespace BinanceDex.Utilities.Logging
{
    public sealed class FileLogger : ILogger
    {
        #region Fields

        #region Private Constants

        private const string Spaces = "       ";

        #endregion Private Constants

        private readonly string _filePath;

        private readonly LogLevel _level;

        private readonly object _sync = new object();

        #endregion

        #region Constructors

        public FileLogger(string filePath, LogLevel level)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentNullException(nameof(filePath));

            this._filePath = filePath;
            this._level = level;
        }

        #endregion

        #region Implements

        public IDisposable BeginScope<TState>(TState state)
        {
            return NullScope.Instance;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            if (logLevel == LogLevel.None)
                return false;

            return logLevel >= this._level;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception,
            Func<TState, Exception, string> formatter)
        {
            if (formatter == null)
                throw new ArgumentNullException(nameof(formatter));

            if (!this.IsEnabled(logLevel))
                return;

            string message = formatter(state, exception);
            if (string.IsNullOrWhiteSpace(message))
                return;

            try
            {
                lock (this._sync)
                {
                    using (FileStream stream = new FileStream(this._filePath, FileMode.Append, FileAccess.Write,
                        FileShare.None))
                    using (StreamWriter streamWriter = new StreamWriter(stream) {AutoFlush = false})
                    {
                        DateTimeOffset now = DateTimeOffset.Now;

                        streamWriter.WriteLine($"[{ConvertLogLevelToString(logLevel)}] {now}");

                        foreach (string line in message.Split(new[] {Environment.NewLine}, StringSplitOptions.None))
                            streamWriter.WriteLine($"{Spaces}{line}");

                        string prefix = string.Empty;

                        while (exception != null)
                        {
                            streamWriter.WriteLine($"{Spaces}{prefix}(exception: \"{exception.Message}\")");

                            prefix += "  ";
                            exception = exception.InnerException;
                        }

                        streamWriter.Flush();
                    }
                }
            }
            catch
            {
                /* ignore */
            }
        }

        #endregion

        #region Private Methods

        private static string ConvertLogLevelToString(LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Trace:
                    return "trce";
                case LogLevel.Debug:
                    return "dbug";
                case LogLevel.Information:
                    return "info";
                case LogLevel.Warning:
                    return "warn";
                case LogLevel.Error:
                    return "fail";
                case LogLevel.Critical:
                    return "crit";
                case LogLevel.None:
                    return "none";
                default:
                    throw new ArgumentOutOfRangeException(nameof(logLevel), logLevel, null);
            }
        }

        #endregion

        #region Private Classes

        private sealed class NullScope : IDisposable
        {
            #region Constructors

            private NullScope()
            {
            }

            #endregion

            #region Properties

            public static NullScope Instance { get; } = new NullScope();

            #endregion

            #region Implements

            public void Dispose()
            {
            }

            #endregion
        }

        #endregion
    }
}