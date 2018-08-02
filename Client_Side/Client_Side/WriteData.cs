using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Client_Side
{
    class WriteData
    {
        private WriteData() { }


        #region Data

        private static string Url;

        private static string WriteMode;

        private static string PathToResults;


        #endregion

        #region Methods

        public static void SendData(List<string> data)
        {
            try
            {
                switch (WriteMode)
                {
                    case "0": { WriteDataInFile(data); break; }
                    case "1": { WriteDataToInfluxDB(data); break; }
                    case "2": { WriteDataInFile(data); WriteDataToInfluxDB(data); break; }
                }
            }
            catch(Exception ex)
            {
                Logger.WriteLog(ex.Message, "error");
                Logger.WriteLog(ex.StackTrace, "error");
            }
        }

        public static bool PingServerAndPort()
        {
            return PingUtil.Ping(Url, false);
        }
        
        private static void WriteDataToInfluxDB(List<string> data)
        {
            try
            {
                data.ForEach(i => WorkWithServer.SendDataToServer(Url, "Post", i));
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex.Message,"error");
                Logger.WriteLog(ex.StackTrace, "error");
            }
        }

        private static void WriteDataInFile(List<string> data)
        {
            int i=0;
        m: try
            {
                File.AppendAllLines(PathToResultsFile, data);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex.Message, "error");
                Logger.WriteLog(ex.StackTrace, "error");
                Logger.WriteLog(String.Format("{0} attempt was failed", i), "info");
                if (i < 3)
                {
                    i++;
                    goto m;
                }                    
            }
        }

        #endregion

        #region Properties

        public static string SetUrl
        {
            set
            {
                Url = value;
            }
        }

        public static string SetDBName
        {
            set
            {
                Url += "/write?db=" + value + "&precision=ms";
            }
        }

        public static string SetWriteMode
        {
            set
            {
                WriteMode = value;
            }
        }

        public static string PathToResultsFile
        {
            get
            {
                return PathToResults;
            }
            set
            {
                PathToResults = value;
            }
        }

        #endregion
    }
}
