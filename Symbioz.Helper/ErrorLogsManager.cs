using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.Helper
{
    public class ErrorLogsManager
    {
        public static string ErrorLogsPath = Environment.CurrentDirectory + "/Logs/";
        static ErrorLogsManager()
        {
            if (!Directory.Exists(ErrorLogsPath))
                Directory.CreateDirectory(ErrorLogsPath);
        }
        public static void AddLog(string exeptionContent,string clientip)
        {
            File.WriteAllText(ErrorLogsPath + GetFileName(), "Client: " + clientip + Environment.NewLine +"At: "+DateTime.Now.ToString()+Environment.NewLine+ exeptionContent);
            Logger.Init2("New LogFile added!");
        }
        public static string GetFileName()
        {
            int count = 1;
            Directory.GetFiles(ErrorLogsPath).ToList().ForEach(x=>count++);
            return count + ".txt";
        }
    }
}
