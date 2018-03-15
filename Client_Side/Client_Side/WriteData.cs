using System;
using System.Collections.Generic;
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

        private static FileStream fs;

        private static string Url;

        private static string WriteMode;

        private static string PathToFile;

        private static bool IsFile = false;

        private static string FilePath;


        #endregion

        #region Methods

        public static void SendData(List<string> data)
        {
            if ((WriteMode == "0") || (WriteMode == "2"))
            {
                if (!IsFile)
                {
                    CreateResultFile();
                }
            }
            try
            {
                switch (WriteMode)
                {
                    case "0": { WriteDataInFile(data); break; }
                    case "1": { WriteDataToInfluxDB(data); break; }
                    case "2": { WriteDataInFile(data); WriteDataToInfluxDB(data); break; }
                }
            }
            catch
            { }
        }

        public static bool PingServerAndPort()
        {
            return PingUtil.Ping(Url, false);
        }

        private static void CreateResultFile()
        {
            string time = String.Format("{0:yyyyddMM_HHmmss}", DateTime.Now).ToString();
            string path = PathToFile + time + ".txt";
            fs = File.Create(path);
            fs.Close();
            FilePath = path;
            IsFile = true;
        }

        private static void WriteDataToInfluxDB(List<string> data)
        {
            try
            {
                data.ForEach(i => WorkWithServer.sendData(Url, "Post", i));
            }
            catch (Exception ex)
            {
                Program.WriteLog(ex.Message);
            }
        }

        private static void WriteDataInFile(List<string> data)
        {
            File.AppendAllLines(FilePath, data);
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

        public static string SetPathToFile
        {
            set
            {
                PathToFile = value;
            }
        }

        #endregion
    }
}
