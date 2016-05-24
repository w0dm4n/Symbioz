using Symbioz.Core.Startup;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.Network.Messages
{
    public class RawDatasManager
    {
        public static Dictionary<string, byte[]> Raws = new Dictionary<string, byte[]>();

        public static string RawRepertory = Environment.CurrentDirectory + "/SWF/";

        [StartupInvoke("RawDatas",StartupInvokeType.Internal)]
        public static void Initialize()
        {
            Raws.Clear();
            foreach (var file in Directory.GetFiles(RawRepertory))
            {
                string rawName = Path.GetFileNameWithoutExtension(file);
                Raws.Add(rawName, File.ReadAllBytes(file));
            }
        }
        public static byte[] GetRawData(string name)
        {
            return Raws.FirstOrDefault(x => x.Key == name).Value;
        }
    }
}
