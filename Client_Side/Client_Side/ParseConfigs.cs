﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Client_Side
{
    class ParseConfigs
    {
        public static bool ParseConfigFile(string pathToFile)
        {
            var data = new Dictionary<string, string>();
            foreach (var row in File.ReadAllLines(pathToFile))
            {
                try
                {
                    if ((row[0] != '#') && (row[0] != ' '))
                    {
                        data.Add(row.Split('=')[0], string.Join("=", row.Split('=').Skip(1).ToArray()));
                    }
                }
                catch
                {
                    continue;
                }
            }

            if (data.Keys.Contains("CheckUrl"))
            {
                Program.ShowMessage("[INFO]\t'CheckUrl' was read successfully\n");
                if (data["CheckUrl"].Contains(','))
                {
                    foreach (var url in data["CheckUrl"].Split(','))
                    {
                        if (!PingUtil.Ping(url, true))
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    if(!PingUtil.Ping(data["CheckUrl"], true))
                    {
                        return false;
                    }
                }
            }

            if (data.Keys.Contains("WriteMode"))
            {
                WriteData.SetWriteMode = data["WriteMode"];
                Program.ShowMessage("[INFO]\t'WriteMode' was read successfully\n");
                if ((data["WriteMode"]=="1") || (data["WriteMode"]=="2"))
                {
                    try
                    {
                        WriteData.SetUrl = data["ServerUrl"];
                        //Tables.SetUrl = data["ServerUrl"];
                        Program.ShowMessage("[INFO]\t'ServerUrl' was read successfully\n");
                        if(!WriteData.PingServerAndPort())
                        {
                            return false;
                        }
                    }
                    catch
                    {
                        Program.ShowMessage("[ERROR]\tParameter 'ServerUrl' wasn't found in config file but it's required!!!");
                        return false;
                    }

                    try
                    {
                        WriteData.SetDBName = data["DBName"];
                        Program.ShowMessage("[INFO]\t'DBName' was read successfully\n");
                    }
                    catch
                    {
                        Program.ShowMessage("[ERROR]\tParameter 'DBName' wasn't found in config file but it's required!!!");
                        return false;
                    }
                }
            }
            else
            {
                WriteData.SetWriteMode = "0";
                Program.ShowMessage("[INFO]\t'WriteMode' parameter was set to default value = 0\n");
            }
    }
}