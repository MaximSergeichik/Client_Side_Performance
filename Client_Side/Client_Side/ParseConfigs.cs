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
                Program.ShowMessageInConsole("'User' was read successfully");
                TestAction.Creditianals = data["User"];
            }
            

            if (data.Keys.Contains("LogPath"))
            {
                if (!Directory.Exists(data["LogPath"]))
                {
                    Directory.CreateDirectory(data["LogPath"]);
                }
                Logger.SetLogFilePath = data["LogPath"];
            }

            if (data.Keys.Contains("CheckUrl"))
            {
                Program.ShowMessageInConsole("'CheckUrl' was read successfully");
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
                Program.ShowMessageInConsole("'WriteMode' was read successfully");

                switch(data["WriteMode"])
                {
                    case "0":
                        {
                            if (data.ContainsKey("OutputFolderPath"))
                            {
                                if (!Directory.Exists(data["OutputFolderPath"]))
                                {
                                    Directory.CreateDirectory(data["OutputFolderPath"]);
                                }
                                DateTime d = DateTime.Now;
                                string s = String.Format("\\ClientSide_{0:yyyyyMMdd-HHmmss}.log",d);
                                WriteData.PathToResultsFile = data["OutputFolderPath"] + s;
                                Program.ShowMessageInConsole(String.Format("Result File will be stored here - {0}", WriteData.PathToResultsFile));
                            }
                            else
                            {
                                DateTime d = DateTime.Now;
                                string s = String.Format("\\ClientSide_{0:yyyyyMMdd-HHmmss}.log", d);
                                WriteData.PathToResultsFile = Directory.GetCurrentDirectory() + s;
                                Program.ShowMessageInConsole(String.Format("Result File will be stored here - {0}", WriteData.PathToResultsFile));
                            }
                            break;
                        }
                    case "1":
                        {
                            WriteData.SetUrl = data["ServerUrl"];
                            //Tables.SetUrl = data["ServerUrl"];
                            Program.ShowMessageInConsole("'ServerUrl' was read successfully");
                            if (!WriteData.PingServerAndPort())
                            {
                                return false;
                            }
                            WriteData.SetDBName = data["DBName"];
                            Program.ShowMessageInConsole("'DBName' was read successfully");
                            break;
                        }
                    case "2":
                        {
                            if (data.ContainsKey("OutputFolderPath"))
                            {
                                if (!Directory.Exists(data["OutputFolderPath"]))
                                {
                                    Directory.CreateDirectory(data["OutputFolderPath"]);
                                }
                                DateTime d = DateTime.Now;
                                string s = String.Format("\\ClientSide_{0:yyyyyMMdd-HHmmss}.log", d);
                                WriteData.PathToResultsFile = data["OutputFolderPath"] + s;
                                Program.ShowMessageInConsole(String.Format("Result File will be stored here - {0}", WriteData.PathToResultsFile));
                            }
                            else
                            {
                                DateTime d = DateTime.Now;
                                string s = String.Format("\\ClientSide_{0:yyyyyMMdd-HHmmss}.log", d);
                                WriteData.PathToResultsFile = Directory.GetCurrentDirectory() + s;
                                Program.ShowMessageInConsole(String.Format("Result File will be stored here - {0}", WriteData.PathToResultsFile));
                            }
                            WriteData.SetUrl = data["ServerUrl"];
                            //Tables.SetUrl = data["ServerUrl"];
                            Program.ShowMessageInConsole("'ServerUrl' was read successfully");
                            if (!WriteData.PingServerAndPort())
                            {
                                return false;
                            }
                            WriteData.SetDBName = data["DBName"];
                            Program.ShowMessageInConsole("'DBName' was read successfully");
                            break;
                        }

                }
            }
            else
            {
                WriteData.SetWriteMode = "0";
                Program.ShowMessageInConsole("'WriteMode' parameter was set to default value = 0");
            }

            try
            {
                TestActionHelp.SetLocation = data["Location"];
                Program.ShowMessageInConsole("'Location' was read successfully");
            }
            catch
            {
                Program.ShowMessageInConsole("Parameter 'Location' wasn't found in config file but it's required!!!");
                return false;
            }

            try
            {
                ParsePlan.SetPathToPlan = data["TestPlan"];
                Program.ShowMessageInConsole("'TestPlan' was read successfully");
            }
            catch
            {
                Program.ShowMessageInConsole("Parameter 'TestPlan' wasn't found in config file but it's required!!!");
                return false;
            }

            if (data.Keys.Contains("Duration"))
            {
                TestActionHelp.SetEndTime = Convert.ToInt32(data["Duration"]);

                Program.ShowMessageInConsole("Parameter 'Duration' was read successfully");
            }
            else if (data.Keys.Contains("IterationCount"))
            {
                TestActionHelp.SetIteration = Convert.ToInt32(data["IterationCount"]);
                Program.ShowMessageInConsole("Parameter 'IterationCount' was read successfully");
            }
            else
            {
                Program.ShowMessageInConsole("No one from this parameters: 'Duration' or 'IterationCount' were not found in config file but it's required!!!");
                return false;
            }

            if (data.Keys.Contains("Threads"))
            {
                TestActionHelp.SetThreadsCount = Convert.ToInt32(data["Threads"]);
                Program.ShowMessageInConsole("Parameter 'Threads' was read successfully");
            }
            else
            {
                TestActionHelp.SetThreadsCount = 1;
                Program.ShowMessageInConsole("Parameter 'Threads' was set to default value which is equal to 1");
            }


            return true;
        }
    }
}
