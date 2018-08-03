using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace Client_Side
{
    class WorkWithServer
    {

        #region Data

        private static WebClient Client = new WebClient();

        #endregion

        #region Methods

        public static void SendDataToServer(string URL, string method, string data)
        {
            try
            {
                Client.UploadData(URL, method, Encoding.Default.GetBytes(data));
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex.Message, "error");
                Logger.WriteLog(ex.StackTrace, "error");
            }
        }

        public static string GetDataFromServer(string URL, string query)
        {
            string response = Client.DownloadString(URL + Uri.EscapeUriString(query));
            return response;
        }

        #endregion
    }
}
