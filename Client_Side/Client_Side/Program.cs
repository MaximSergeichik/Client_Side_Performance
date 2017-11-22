using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Client_Side
{
    class Program
    {
        public static void ShowMessage(string s)
        {
            Console.WriteLine(s);
        }

        public static void WriteLog(string s)
        {
            File.AppendAllText(@"D:/ClientSideLog.txt", s + "\n");
        }

        static void Main(string[] args)
        {
        }
    }
}
