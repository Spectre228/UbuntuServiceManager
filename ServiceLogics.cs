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
            if (selItm.Contains(status) && status.Equals(" active "))
            {
                return "This service is already in running process \n";
            }
            else if (selItm.Contains(status) && status.Equals(" inactive "))
            {
                return "This service has already finished its run \n";
            }

            var proc = new Process();
            var statChecker = new Process();

            proc.StartInfo = new ProcessStartInfo("service", (selItm).Split(".")[0]/*.Remove(0, 2)*/ + " " + action);
            statChecker.StartInfo = new ProcessStartInfo("service", (selItm).Split(".")[0]/*.Remove(0, 2)*/ + " status")
            {
                RedirectStandardOutput = true
            };

            proc.Start();
            proc.WaitForExit();

            statChecker.Start();

            string res = "";
            while (!statChecker.StandardOutput.EndOfStream)
            {
                res += statChecker.StandardOutput.ReadLine() + "\n";
            }

            statChecker.WaitForExit();

            return res;
        }

        //private static List<string>? servs;
        
        private static string whiteSpaceFormator(string nextOutputStr)
        {
        	    nextOutputStr = nextOutputStr.Remove(75).Remove(0, 2);
        	    
                    for (int i=0; i<nextOutputStr.Length; i++)
                    {
                    	if (nextOutputStr[i].Equals(' ') && i<53)
                    	{
                    	    for (int j=i; j<=53; j++)
                    	    {
                    	    	nextOutputStr=nextOutputStr.Insert(j, " ");
                    	    }
                    	    break;
                    	}
                    }
                    
                    return nextOutputStr;
        }

        public static List<string> getServiceList(string searchReq)
        {
            var proc = new Process();
            proc.StartInfo = new ProcessStartInfo("systemctl", "list-units -t service --all")
            {
                RedirectStandardOutput = true
            };

            proc.Start();

            List<string> servs = new List<string>();
            string nextOutputStr = " ";

            if (String.IsNullOrWhiteSpace(searchReq))
            {
                int firstIterSkiped = 0;
                while (!String.IsNullOrEmpty(nextOutputStr = proc.StandardOutput.ReadLine()))
                {
                    //if (firstIterSkiped==0)
                    //{ continue; }
                    
                    firstIterSkiped++;
                    
                    nextOutputStr = whiteSpaceFormator(nextOutputStr);
                    servs.Add(nextOutputStr);
                }

                proc.WaitForExit();
            }
            else
            {
                while (!String.IsNullOrEmpty(nextOutputStr = proc.StandardOutput.ReadLine()))
                {
                    if (nextOutputStr.Split(".")[0].Remove(0, 2).Contains(searchReq)) // Determining whether SERVICE NAME contains search request string
                    {
                    	nextOutputStr = whiteSpaceFormator(nextOutputStr);
                        servs.Add(nextOutputStr);
                    }
                }

                proc.WaitForExit();
            }
            return servs;
        }

        public static string requestHandler(string selItm, string action)
        {
            switch (action)
            {
                    case "start" : 
                    {
                        return requestPerformer(selItm, " active ", action);
                    }
                    case "stop" : 
                    {
                        return requestPerformer(selItm, " inactive ", action);
                    }
                    case "restart" :
                    {
                        return requestPerformer(selItm, " inactive ", action);
                    }
                    default :
                    {
                        return "We will never get into here \n";
                    }
            }
        }
    }
}