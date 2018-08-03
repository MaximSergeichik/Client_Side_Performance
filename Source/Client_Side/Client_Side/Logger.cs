using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using NLog.Config;

namespace Client_Side
{
    
    static class Logger
    {
        #region Data

        private static string LogFilePath;
        

        #endregion

        #region Properties
           
        public static string SetLogFilePath
        {
            set
            {
                DateTime d = DateTime.Now;
                
                string s = String.Format("{0:yyyyMMdd-HHmmss}",d);
                LogFilePath = value + "\\ClientSideLog_" + s + ".log";
                var config = new NLog.Config.LoggingConfiguration();

                var logFile = new NLog.Targets.FileTarget("logfile") { FileName = LogFilePath };
                config.AddRule(NLog.LogLevel.Info, NLog.LogLevel.Fatal, logFile);
                NLog.LogManager.Configuration = config;
            }
        }

        #endregion

        #region Methods

        public static void WriteLog(string message, string level)
        {
            var logger = NLog.LogManager.GetCurrentClassLogger();
            switch(level)
            {
                case "info": { logger.Info(message); break; }
                case "error": { logger.Error(message); break; }
            }
        }

        #endregion

    }
}
