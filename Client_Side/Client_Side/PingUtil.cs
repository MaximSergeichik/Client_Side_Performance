using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace Client_Side
{
    class PingUtil
    {
        private static string rUrl = "http.*[//]{2}([a-z0-9].*[.][a-z].*)$";
        private static string rIP = "([0-9]{1,3}[.][0-9]{1,3}[.][0-9]{1,3}[.][0-9]{1,3})";
        private static string rHost = "[//]{2}(.*?)[:]";
        private static string rPort = "[:]([0-9]{1,4})";

        public static bool Ping(string check, bool isUrl)
        {
            if (isUrl)
            {
                return CheckUrl(check);
            }
            else
            {
                return CheckIPAndPort(check);
            }
        }

        private static bool CheckUrl(string url)
        {
            Regex reg = new Regex(rUrl);
            string u = reg.Match(url).Groups[1].Value.ToString();

            if (Check(u))
            {
                Program.ShowMessage("[INFO]\t" + url + " is avaliable!");
                return true;
            }
            else
            {
                Program.ShowMessage("[ERROR]\t" + url + " is not avaliable!");
                return false;
            }
        }

        private static bool CheckIPAndPort(string url)
        {
            Regex reg = new Regex(rIP);
            Match m = reg.Match(url);

            string ip = m.Groups[1].Value;

            if (ip == "")
            {
                reg = new Regex(rHost);

                m = reg.Match(url);

                ip = m.Groups[1].Value;
            }

            if (Check(ip))
            {
                reg = new Regex(rPort);
                m = reg.Match(url);
                int port = Convert.ToInt16(m.Groups[1].Value);
                Program.ShowMessage("\t[INFO]\tThis ip: " + ip + " is avaliable!\n");
                if (CheckPort(ip, port))
                {
                    Program.ShowMessage("\t[INFO]\tThis port: " + port + " is avaliable!\n");
                    return true;
                }
                else
                {
                    Program.ShowMessage("\t[ERROR]\tThis port on this url: " + ip + " is not avaliable!!!\n");
                    return false;
                }
            }
            else
            {
                Program.ShowMessage("\t[ERROR]\tThis ip: " + ip + "is not avaliable!!!\n");
                return false;
            }
        }

        private static bool CheckPort(string ip, int port)
        {
            TcpClient cl = new TcpClient();
            try
            {
                cl.Connect(ip, Convert.ToInt32(port));
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static bool Check(string addr)
        {
            Ping ping = new Ping();
            PingReply reply = null;
            reply = ping.Send(addr);

            if (reply.Status == IPStatus.Success)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
