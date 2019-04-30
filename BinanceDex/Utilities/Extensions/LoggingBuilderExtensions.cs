using System;
using BinanceDex.Utilities.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace BinanceDex.Utilities.Extensions
{
    public static class LoggingBuilderExtensions
    {
        #region Methods

        public static ILoggingBuilder AddFile(this ILoggingBuilder builder, IConfiguration configuration)
        {
            Throw.IfNull(configuration, nameof(configuration));

            const string filePathSectionKey = "Path";

            IConfigurationSection filePathSection = configuration.GetSection(filePathSectionKey);

            if (filePathSection == null) throw new Exception($"File logger configuration does not contain a '{filePathSectionKey}' section.");

            string filePath = filePathSection.Value;

            if (filePath.IsNullOrWhiteSpace()) throw new Exception($"File logger configuration '{filePathSectionKey}' section does not contain a value.");

            LogLevel level = LogLevel.None;

            IConfigurationSection logLevelSection = configuration.GetSection("LogLevel");
            string defaultLogLevel = logLevelSection?["Default"];

            if (string.IsNullOrWhiteSpace(defaultLogLevel)) return AddFile(builder, filePath, level);

            if (!Enum.TryParse(defaultLogLevel, out level))
            {
                throw new InvalidOperationException($"Configuration value '{defaultLogLevel}' for category 'Default' is not supported.");
            }

            return AddFile(builder, filePath, level);
        }

        public static ILoggingBuilder AddFile(this ILoggingBuilder builder, string filePath, LogLevel level = LogLevel.Information)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder), $"{nameof(ILoggerFactory)} is null (add Microsoft.Extensions.Logging NuGet package).");

            if (filePath == null) throw new ArgumentNullException(nameof(filePath));

            builder.AddProvider(new FileLoggerProvider(filePath, level));

            return builder;
        }

        #endregion
    }
}