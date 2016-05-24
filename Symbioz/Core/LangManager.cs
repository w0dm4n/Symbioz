using Symbioz.Core.Startup;
using Symbioz.DofusProtocol.D2I;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.Core
{
    public class LangManager
    {
        public static string GetText(int id)
        {
            string str = D2IFile.GetText(id);
            if (str == string.Empty || str == null)
                return "[UNKNOWN_TEXT_ID_" + id + "]";
            else return str;
        }
        private static D2IFile D2IFile { get; set; }

        [StartupInvoke("D2I File",StartupInvokeType.Base)]
        public static void Intialize()
        {
            D2IFile = new D2IFile();
            D2IFile.Open(Environment.CurrentDirectory + "\\lang.d2i");
        }
    }
}
