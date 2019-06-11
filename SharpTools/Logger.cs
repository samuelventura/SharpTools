using System;
using System.IO;
using System.Threading;
using System.Collections.Generic;

namespace SharpTools
{
    public enum LogLevel
    {
        DEBUG,
        INFO,
        WARN,
        ERROR,
        SUCCESS,
    }

    public class Log
    {
        public string Name;
        public DateTime Timestamp;
        public LogLevel Level;
        public string ThreadName;
        public int Verbosity;
        public string Format;
        public object[] Args;
        public string Message;
        public string Line;
    }

    public interface ILogger
    {
        void Log(LogLevel level, int Verbosity, string format, params object[] args);

        void Debug(int verbosity, string format, params object[] args);

        void Debug(string format, params object[] args);

        void Info(string format, params object[] args);

        void Warn(string format, params object[] args);

        void Error(string format, params object[] args);

        void Success(string format, params object[] args);
    }

    public interface ILogAppender
    {
        void Append(Log log);
    }

    public interface ILogFormatter
    {
        void Format(Log log);
    }

    public class Logger : ILogger
    {
        private readonly LogRunner runner;
        private readonly string name;
        private readonly Func<DateTime> timer;

        public Logger(LogRunner runner, string name = null, Func<DateTime> timer = null)
        {
            this.timer = timer ?? Now;
            this.runner = runner;
            this.name = name;
        }

        public void Debug(int verbosity, string format, params object[] args)
        {
            Log(LogLevel.DEBUG, verbosity, format, args);
        }

        public void Debug(string format, params object[] args)
        {
            Log(LogLevel.DEBUG, 0, format, args);
        }

        public void Info(string format, params object[] args)
        {
            Log(LogLevel.INFO, 0, format, args);
        }

        public void Warn(string format, params object[] args)
        {
            Log(LogLevel.WARN, 0, format, args);
        }

        public void Error(string format, params object[] args)
        {
            Log(LogLevel.ERROR, 0, format, args);
        }

        public void Success(string format, params object[] args)
        {
            Log(LogLevel.SUCCESS, 0, format, args);
        }

        public void Log(LogLevel level, int verbosity, string format, params object[] args)
        {
            runner.Log(new Log
            {
                Name = name,
                Level = level,
                Timestamp = timer(),
                ThreadName = Thread.CurrentThread.Name,
                Verbosity = verbosity,
                Format = format,
                Args = args
            });
        }

        private DateTime Now()
        {
            return DateTime.Now;
        }
    }

    public class LogRunner : IDisposable
    {
        private readonly List<Action<Log>> appenders;
        private readonly List<Action<Log>> removes;
        private readonly ILogFormatter formatter;
        private readonly ThreadRunner runner;

        public LogRunner(ILogFormatter formatter = null)
        {
            this.appenders = new List<Action<Log>>();
            this.removes = new List<Action<Log>>();
            this.formatter = formatter ?? PatternLogFormatter.TIMEONLY_MESSAGE;
            //only one per application expected
            this.runner = new ThreadRunner("LogRunner");
        }

        public void Dispose()
        {
            //remove reference, dispose 
            runner.Dispose(appenders.Clear);
        }

        public void AddAppender(ILogAppender appender)
        {
            runner.Run(() => appenders.Add(appender.Append));
        }

        public void AddAppender(Action<Log> appender)
        {
            runner.Run(() => appenders.Add(appender));
        }

        public void Log(Log log)
        {
            formatter.Format(log);

            runner.Run(() =>
            {
                foreach (var appender in appenders)
                {
                    Catcher.Try(() => appender(log), (ex) =>
                    {
                        removes.Add(appender);
                        Thrower.Dump(ex);
                    });
                }
                //autoremove excepting appenders
                foreach (var appender in removes)
                {
                    appenders.Remove(appender);
                }
                removes.Clear();
            });
        }
    }

    public class PatternLogFormatter : ILogFormatter
    {
        public readonly static ILogFormatter LINE = new PatternLogFormatter("{MESSAGE}");
        public readonly static ILogFormatter TIMEONLY_MESSAGE = new PatternLogFormatter("{TIMESTAMP:HH:mm:ss.fff} {MESSAGE}");
        public readonly static ILogFormatter TIMESTAMP_MESSAGE = new PatternLogFormatter("{TIMESTAMP:yyyy-MM-dd HH:mm:ss.fff} {MESSAGE}");
        public readonly static ILogFormatter TIMESTAMP_LEVEL_MESSAGE = new PatternLogFormatter("{TIMESTAMP:yyyy-MM-dd HH:mm:ss.fff} {LEVEL} {MESSAGE}");
        public readonly static ILogFormatter TIMESTAMP_LEVEL_THREAD_MESSAGE = new PatternLogFormatter("{TIMESTAMP:yyyy-MM-dd HH:mm:ss.fff} {LEVEL} {THREAD} {MESSAGE}");
        public readonly static ILogFormatter TIMESTAMP_LEVEL_NAME_MESSAGE = new PatternLogFormatter("{TIMESTAMP:yyyy-MM-dd HH:mm:ss.fff} {LEVEL} {NAME} {MESSAGE}");

        private readonly string format;

        public PatternLogFormatter(string pattern)
        {
            pattern = pattern.Replace("NAME", "0");
            pattern = pattern.Replace("TIMESTAMP", "1");
            pattern = pattern.Replace("VERBOSITY", "2");
            pattern = pattern.Replace("LEVEL", "3");
            pattern = pattern.Replace("THREAD", "4");
            pattern = pattern.Replace("MESSAGE", "5");
            this.format = pattern;
        }

        public void Format(Log log)
        {
            log.Message = string.Format(log.Format, log.Args);
            var args = new object[] { log.Name, log.Timestamp, log.Verbosity, log.Level, log.ThreadName, log.Message };
            log.Line = string.Format(format, args);
        }
    }

    public class ConsoleLogAppender : ILogAppender
    {
        public void Append(Log log)
        {
            //console appenders honor debug by default
            Console.WriteLine(log.Line);
        }
    }

    public class WriterLogAppender : ILogAppender, IDisposable
    {
        private readonly TextWriter writer;

        public WriterLogAppender(string filePath)
        {
            this.writer = new StreamWriter(filePath, true);
        }

        public WriterLogAppender(TextWriter writer)
        {
            this.writer = writer;
        }

        public void Append(Log log)
        {
            //file appenders ignore debug by default
            if (log.Level != LogLevel.DEBUG)
            {
                writer.WriteLine(log.Line);
                writer.Flush();
            }
        }

        public void Dispose()
        {
            Disposer.Dispose(writer);
        }
    }
}
