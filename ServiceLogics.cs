using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace ServiceManager
{

    static internal class ServiceLogics
    {
        private static string requestPerformer(string selItm, string status, string action)
        {
            if ((selItm).Remove(0, 70).Contains(status) && status.Equals("running"))
            {
                return "This service is already in running process \n";
            }
            else if ((selItm).Remove(0, 70).Contains(status) && status.Equals("exited"))
            {
                return "This service has already finished its run \n";
            }

            var proc = new Process();
            var statChecker = new Process();

            proc.StartInfo = new ProcessStartInfo("service", (selItm).Split(".")[0].Remove(0, 2) + " " + action);
            statChecker.StartInfo = new ProcessStartInfo("service", (selItm).Split(".")[0].Remove(0, 2) + " status")
            {
                RedirectStandardOutput = true
            };

            proc.Start();
            proc.WaitForExit();

            statChecker.Start();

            string res = "";
            while (!statChecker.StandardOutput.EndOfStream)
            {
                res += statChecker.StandardOutput.ReadLine();
            }

            statChecker.WaitForExit();

            return res;
        }

        //private static List<string>? servs;

        public static void getServiceList(string searchReq, ref List<string> servs, ref TextBlock OutputLog)
        {
            var proc = new Process();
            proc.StartInfo = new ProcessStartInfo("systemctl", "list-units -t service")
            {
                RedirectStandardOutput = true
            };

            proc.Start();

            servs = new List<string>();
            string nextOutputStr = " ";

            if (String.IsNullOrWhiteSpace(searchReq))
            {
                while (!String.IsNullOrEmpty(nextOutputStr = proc.StandardOutput.ReadLine()))
                {
                    servs.Add(nextOutputStr.Remove(78));
                    //OutputLog.Text += servs[^1] + "\n";
                    //Console.WriteLine(servs[^1]);
                }

                proc.WaitForExit();
            }
            else
            {
                while (!String.IsNullOrEmpty(nextOutputStr = proc.StandardOutput.ReadLine()))
                {
                    if (nextOutputStr.Split(".")[0].Remove(0, 2).Contains(searchReq)) // Determining whether SERVICE NAME contains search request string
                    {
                        servs.Add(nextOutputStr.Remove(78));
                        //OutputLog.Text += servs[^1] + "\n";
                        //Console.WriteLine(servs[^1]);
                    }
                }

                proc.WaitForExit();
            }
        }

        public static string requestHandler(string selItm, string action)
        {
            switch (action)
            {
                    case "start" : 
                    {
                        return requestPerformer(selItm, "running", action);
                    }
                    case "stop" : 
                    {
                        return requestPerformer(selItm, "exited", action);
                    }
                    case "restart" :
                    {
                        return requestPerformer(selItm, "", action);
                    }
                    default :
                    {
                        return "We will never get into here \n";
                    }
            }
        }
    }
}