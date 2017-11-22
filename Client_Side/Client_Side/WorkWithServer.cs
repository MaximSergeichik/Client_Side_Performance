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
        private static WebClient cl = new WebClient();

        public static void sendData(string URL, string method, string data)
        {
            try
            {
                cl.UploadData(URL, method, Encoding.Default.GetBytes(data));
            }
            catch (Exception ex)
            {
                Program.WriteLog(ex.Message);
            }
        }

        public static string getData(string URL, string query)
        {
            string response = cl.DownloadString(URL + Uri.EscapeUriString(query));
            return response;
        }
    }
}
