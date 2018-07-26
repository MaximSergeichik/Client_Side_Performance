using System;
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


            if (data.Keys.Contains("User"))
            {
                Program.ShowMessageInConsole("[INFO]\t'User' was read successfully\n");
                TestAction.Creditianals = data["User"];
            }
            

            if (data.Keys.Contains("CheckUrl"))
            {
                Program.ShowMessageInConsole("[INFO]\t'CheckUrl' was read successfully\n");
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
                    if (!PingUtil.Ping(data["CheckUrl"], true))
                    {
                        return false;
                    }
                }
            }

            if (data.Keys.Contains("WriteMode"))
            {
                WriteData.SetWriteMode = data["WriteMode"];
                Program.ShowMessageInConsole("[INFO]\t'WriteMode' was read successfully\n");
                if ((data["WriteMode"] == "1") || (data["WriteMode"] == "2"))
                {
                    try
                    {
                        WriteData.SetUrl = data["ServerUrl"];
                        //Tables.SetUrl = data["ServerUrl"];
                        Program.ShowMessageInConsole("[INFO]\t'ServerUrl' was read successfully\n");
                        if (!WriteData.PingServerAndPort())
                        {
                            return false;
                        }
                    }
                    catch
                    {
                        Program.ShowMessageInConsole("[ERROR]\tParameter 'ServerUrl' wasn't found in config file but it's required!!!");
                        return false;
                    }

                    try
                    {
                        WriteData.SetDBName = data["DBName"];
                        Program.ShowMessageInConsole("[INFO]\t'DBName' was read successfully\n");
                    }
                    catch
                    {
                        Program.ShowMessageInConsole("[ERROR]\tParameter 'DBName' wasn't found in config file but it's required!!!");
                        return false;
                    }
                }
            }
            else
            {
                WriteData.SetWriteMode = "0";
                Program.ShowMessageInConsole("[INFO]\t'WriteMode' parameter was set to default value = 0\n");
            }

            try
            {
                TestActionHelp.SetLocation = data["Location"];
                Program.ShowMessageInConsole("[INFO]\t'Location' was read successfully\n");
            }
            catch
            {
                Program.ShowMessageInConsole("[ERROR]\tParameter 'Location' wasn't found in config file but it's required!!!");
                return false;
            }

            try
            {
                ParsePlan.SetPathToPlan = data["TestPlan"];
                Program.ShowMessageInConsole("[INFO]\t'TestPlan' was read successfully\n");
            }
            catch
            {
                Program.ShowMessageInConsole("[ERROR]\tParameter 'TestPlan' wasn't found in config file but it's required!!!");
                return false;
            }

            if (data.Keys.Contains("Duration"))
            {
                TestActionHelp.SetEndTime = Convert.ToInt32(data["Duration"]);

                Program.ShowMessageInConsole("[INFO]\tParameter 'Duration' was read successfully\n");
            }
            else if (data.Keys.Contains("IterationCount"))
            {
                TestActionHelp.SetIteration = Convert.ToInt32(data["IterationCount"]);
                Program.ShowMessageInConsole("[INFO]\tParameter 'IterationCount' was read successfully\n");
            }
            else
            {
                Program.ShowMessageInConsole("[ERROR]\tNo one from this parameters: 'Duration' or 'IterationCount' were not found in config file but it's required!!!");
                return false;
            }

            if (data.Keys.Contains("Threads"))
            {
                TestActionHelp.SetThreadsCount = Convert.ToInt32(data["Threads"]);
                Program.ShowMessageInConsole("[INFO]\tParameter 'Threads' was read successfully\n");
            }
            else
            {
                TestActionHelp.SetThreadsCount = 1;
                Program.ShowMessageInConsole("[INFO]\tParameter 'Threads' was set to default value which is equal to 1\n");
            }


            return true;
        }
    }
}
