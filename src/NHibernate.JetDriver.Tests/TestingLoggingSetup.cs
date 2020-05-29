using System;
using System.Reflection;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Layout;
using log4net.Repository;
using log4net.Repository.Hierarchy;

namespace NHibernate.JetDriver.Tests
{
    public class TestingLoggingSetup
    {
        private static readonly object Lock = new object();
        private static bool _configured;

        public static void Configure()
        {
            lock (Lock)
            {
                if (_configured)
                {
                    return;
                }

                ILoggerRepository repository = LogManager.GetRepository(Assembly.GetExecutingAssembly());
                BasicConfigurator.Configure(repository, new TraceAppender()
                {
                    Layout = new PatternLayout(PatternLayout.DetailConversionPattern),
                });

                ISetLoggerLevel("NHibernate", Level.Info);
                ISetLoggerLevel("NHibernate.SQL", Level.Debug);

                _configured = true;
            }
        }

        public static void ISetLoggerLevel(System.Type type, Level level)
        {
            ILog log = LogManager.GetLogger(type);
            ISetLoggerLevel(log, level);
        }
        public static void ISetLoggerLevel(string loggerName, Level level)
        {
            ILog log = LogManager.GetLogger(Assembly.GetExecutingAssembly(), loggerName);
            ISetLoggerLevel(log, level);
        }
        public static void ISetLoggerLevel(ILog logger, Level level)
        {
            Logger hierarchyLogger = (Logger)logger.Logger;
            ISetLoggerLevel(hierarchyLogger, level);
        }
        public static void ISetLoggerLevel(Logger hierarchyLogger, Level level)
        {
            hierarchyLogger.Level = level;
        }

    }
}