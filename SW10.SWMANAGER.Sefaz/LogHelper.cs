using log4net;
using log4net.Config;
using log4net.Appender;
using log4net.Layout;
using System.Collections.Concurrent;
using System;

namespace Sefaz
{
    public static class SefazLogHelper
    {
        private static readonly ConcurrentDictionary<string, ILog> loggers = new ConcurrentDictionary<string, ILog>();

        static SefazLogHelper()
        {
            XmlConfigurator.Configure();
        }

        public static void Init(string cnpj)
        {
            XmlConfigurator.Configure();
            GetLoggerInternal(cnpj);
        }


        public static void Debug(string cnpj, string message)
        {
            GetLoggerInternal(cnpj).Debug(message);
        }

        public static void Error(string cnpj, string message)
        {
            GetLoggerInternal(cnpj).Error(message);
        }

        public static void Info(string cnpj, string message)
        {
            GetLoggerInternal(cnpj).Info(message);
        }

        public static void Warn(string cnpj, string message)
        {
            GetLoggerInternal(cnpj).Warn(message);
        }

        public static void Debug(string cnpj, string message, Exception e)
        {
            GetLoggerInternal(cnpj).Debug(message, e);
        }

        public static void Error(string cnpj, string message, Exception e)
        {
            GetLoggerInternal(cnpj).Error(message, e);
        }

        public static void Info(string cnpj, string message, Exception e)
        {
            GetLoggerInternal(cnpj).Info(message, e);
        }

        public static void Warn(string cnpj, string message, Exception e)
        {
            GetLoggerInternal(cnpj).Warn(message, e);
        }

        private static ILog GetLoggerInternal(string logger)
        {
            if (!loggers.ContainsKey(logger))
            {
                var appender = CreateRollingFileAppender(logger);
                appender.ActivateOptions();
                loggers.TryAdd(logger, LogManager.GetLogger(logger));
                ((log4net.Repository.Hierarchy.Logger)loggers[logger].Logger).AddAppender(appender);
            }
            return loggers[logger];
        }

        private static RollingFileAppender CreateRollingFileAppender(string cnpj)
        {
            var layout = new PatternLayout
            {
                ConversionPattern = "%date %-5level - %message%newline"
            };
            layout.ActivateOptions();

            return new RollingFileAppender
            {
                Name = cnpj,
                AppendToFile = true,
                DatePattern = "ddMMyyyy",
                PreserveLogFileNameExtension = true,
                MaximumFileSize = "1MB",
                MaxSizeRollBackups = 10,
                RollingStyle = RollingFileAppender.RollingMode.Date,
                File = $".\\Logs\\Sefaz\\{cnpj}\\log.txt",
                Layout = layout
            };
        }
    }
}
