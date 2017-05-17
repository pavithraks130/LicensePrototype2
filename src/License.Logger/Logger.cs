using System;
using System.IO;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace Logger
{
    public class Logger
    {
        private static readonly NLog.Logger _logger = LogManager.GetCurrentClassLogger();

        public static void Info(string message)
        {
            _logger.Info(message);
        }

        public static void Warn(string message)
        {
            _logger.Warn(message);
        }

        public static void Debug(string message)
        {
            _logger.Debug(message);
        }

        public static void Error(string message)
        {
            _logger.Error(message);
        }

        public static void Error(Exception x)
        {
            _logger.Error(LoggerExt.BuildExceptionMessage(x));
        }

        public static void Fatal(string message)
        {
            _logger.Fatal(message);
        }

        public static void Fatal(Exception x)
        {
            _logger.Fatal(LoggerExt.BuildExceptionMessage(x));
        }

        public static void ConfigureLogger(string path,string filename)
        {
            // Step 1. Create configuration object 
            var config = new LoggingConfiguration();

            var fileTarget = new FileTarget();
            config.AddTarget("file", fileTarget);


            //   path ="E:\\SAWDGS\\";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            fileTarget.FileName = Path.Combine(path, filename+".log");
            fileTarget.ArchiveFileName = Path.Combine(path, filename+".{#####}.txt");
            fileTarget.ArchiveAboveSize = 104857600; //102,400KB = 100MB   //10240; // 10kb
            fileTarget.ArchiveNumbering = ArchiveNumberingMode.Sequence;
            fileTarget.ConcurrentWrites = true;
            fileTarget.KeepFileOpen = false;

            fileTarget.Layout = "${longdate} | ${level} | ${message}";

            var rule2 = new LoggingRule("*", LogLevel.Info, fileTarget);
            config.LoggingRules.Add(rule2);

            // Step 5. Activate the configuration
            LogManager.Configuration = config;
        }

        //public static string GetAppLogPath()
        //{
        //    var logPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        //       Global.MEMORYDRIVENAME);
        //    return logPath;
        //}
    }

    public static class LoggerExt
    {
        public static string BuildExceptionMessage(Exception x)
        {
            Exception logException = x;
            if (x.InnerException != null)
                logException = x.InnerException;

            string strErrorMsg = Environment.NewLine + "Message :" + logException.Message;

            strErrorMsg += Environment.NewLine + "Source :" + logException.Source;

            strErrorMsg += Environment.NewLine + "Stack Trace :" + logException.StackTrace;

            strErrorMsg += Environment.NewLine + "TargetSite :" + logException.TargetSite;
            return strErrorMsg;
        }
    }
}
