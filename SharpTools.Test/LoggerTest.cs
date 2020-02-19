using System;
using System.IO;
using System.Collections.Generic;
using System.Threading;
using Xunit;

namespace SharpTools
{
    public class LoggerTest
    {
        private class StringAppender : ILogAppender
        {
            public readonly List<Log> Logs = new List<Log>();
            public void Append(Log log)
            {
                Logs.Add(log);
            }
        }

        private class ExceptionAppender : ILogAppender
        {
            public readonly List<Log> Logs = new List<Log>();
            public void Append(Log log)
            {
                if (log.Level == LogLevel.WARN) throw new Exception("Unsupported WARN level");
                Logs.Add(log);
            }
        }

        [Fact]
        public void BasicTest()
        {
            var appender = new StringAppender();

            using (var runner = new LogRunner(new PatternLogFormatter("{LEVEL} {NAME} {MESSAGE}")))
            {
                runner.AddAppender(appender);

                var logger = new Logger(runner, "N$AME");

                logger.Debug("Message {0}", 1);
                logger.Info("Message {0}", 2);
                logger.Warn("Message {0}", 3);
                logger.Error("Message {0}", 4);
                logger.Success("Message {0}", 5);
            }

            Assert.Equal(5, appender.Logs.Count);
            Assert.Equal("DEBUG N$AME Message 1", appender.Logs[0].Line);
            Assert.Equal("INFO N$AME Message 2", appender.Logs[1].Line);
            Assert.Equal("WARN N$AME Message 3", appender.Logs[2].Line);
            Assert.Equal("ERROR N$AME Message 4", appender.Logs[3].Line);
            Assert.Equal("SUCCESS N$AME Message 5", appender.Logs[4].Line);
        }

        [Fact]
        public void AppenderExceptionTest()
        {
            var appender = new ExceptionAppender();

            using (var runner = new LogRunner(new PatternLogFormatter("{LEVEL} {NAME} {MESSAGE}")))
            {
                runner.AddAppender(appender);

                var logger = new Logger(runner, "N$AME");

                logger.Debug("Message {0}", 1);
                logger.Info("Message {0}", 2);
                logger.Warn("Message {0}", 3);
                logger.Error("Message {0}", 4);
                logger.Success("Message {0}", 5);
            }

            Assert.Equal(2, appender.Logs.Count);
            Assert.Equal("DEBUG N$AME Message 1", appender.Logs[0].Line);
            Assert.Equal("INFO N$AME Message 2", appender.Logs[1].Line);
            //discarded from WARN on
        }

        [Fact]
        public void ThreadTimestampVerbosityTest()
        {
            var appender = new StringAppender();
            using (var runner = new LogRunner(new PatternLogFormatter("{THREAD} {VERBOSITY} {TIMESTAMP:yyyy-MM-dd HH:mm:ss.fff} {MESSAGE}")))
            {
                runner.AddAppender(appender);

                var logger = new Logger(runner, "nunit", () => { return DateTime.Parse("2019-01-02 23:24:25.876"); });

                logger.Debug("Message {0}", 1);
                logger.Debug(1, "Message {0}", 2);
            }

            Assert.Equal(2, appender.Logs.Count);
            Assert.Equal(Thread.CurrentThread.Name + " 0 2019-01-02 23:24:25.876 Message 1", appender.Logs[0].Line);
            Assert.Equal(Thread.CurrentThread.Name + " 1 2019-01-02 23:24:25.876 Message 2", appender.Logs[1].Line);
        }

        [Fact]
        public void ConsoleAppenderTest()
        {
            var lines = Program.Run("Logger");

            Assert.Equal(5, lines.Length);
            Assert.Equal("DEBUG N$AME Message 1", lines[0]);
            Assert.Equal("INFO N$AME Message 2", lines[1]);
            Assert.Equal("WARN N$AME Message 3", lines[2]);
            Assert.Equal("ERROR N$AME Message 4", lines[3]);
            Assert.Equal("SUCCESS N$AME Message 5", lines[4]);
        }

        [Fact]
        public void FileAppenderTest()
        {
            var file = Executable.Relative("Logs", "nunit-log.txt");
            Directory.CreateDirectory(Executable.Relative("Logs"));
            File.Delete(file);
            while (File.Exists(file)) Thread.Yield();

            //order matters, dispose runner then appender
            using (var appender = new WriterLogAppender(file))
            using (var runner = new LogRunner(new PatternLogFormatter("{LEVEL} {NAME} {MESSAGE}")))
            {
                runner.AddAppender(appender);

                var logger = new Logger(runner, "N$AME");

                logger.Debug("Message {0}", 1);
                logger.Info("Message {0}", 2);
                logger.Warn("Message {0}", 3);
                logger.Error("Message {0}", 4);
                logger.Success("Message {0}", 5);
            }

            var lines = File.ReadAllLines(file);

            //no DEBUG to file
            Assert.Equal(4, lines.Length);
            Assert.Equal("INFO N$AME Message 2", lines[0]);
            Assert.Equal("WARN N$AME Message 3", lines[1]);
            Assert.Equal("ERROR N$AME Message 4", lines[2]);
            Assert.Equal("SUCCESS N$AME Message 5", lines[3]);
        }

        [Fact]
        public void StaticFormattersTest()
        {
            var log = new Log
            {
                Name = "NAM$",
                Level = LogLevel.INFO,
                Timestamp = DateTime.Parse("2019-01-02 23:24:25.876"),
                ThreadName = "THREA$",
                Verbosity = 1,
                Format = "Message {0}",
                Args = new object[] { 2 }
            };

            PatternLogFormatter.LINE.Format(log);
            Assert.Equal("Message 2", log.Line);
            PatternLogFormatter.TIMEONLY_MESSAGE.Format(log);
            Assert.Equal("23:24:25.876 Message 2", log.Line);
            PatternLogFormatter.TIMESTAMP_MESSAGE.Format(log);
            Assert.Equal("2019-01-02 23:24:25.876 Message 2", log.Line);
            PatternLogFormatter.TIMESTAMP_LEVEL_MESSAGE.Format(log);
            Assert.Equal("2019-01-02 23:24:25.876 INFO Message 2", log.Line);
            PatternLogFormatter.TIMESTAMP_LEVEL_THREAD_MESSAGE.Format(log);
            Assert.Equal("2019-01-02 23:24:25.876 INFO THREA$ Message 2", log.Line);
            PatternLogFormatter.TIMESTAMP_LEVEL_NAME_MESSAGE.Format(log);
            Assert.Equal("2019-01-02 23:24:25.876 INFO NAM$ Message 2", log.Line);
        }
    }
}