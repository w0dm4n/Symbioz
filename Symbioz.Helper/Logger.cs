using Symbioz.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz
{
    public class Logger
    {
        public const string LogSymbol = "))";

        public static void OnStartup()
        {
            Console.Title = "Hestia";
            Logger.Init2("Version " + ConstantsRepertory.VERSION);
        }
        public static void NewLine()
        {
            Console.WriteLine(Environment.NewLine);
        }
        public static void Error(object value)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Error: " + value, ConsoleColor.Red);
            //Write("Error: "+ value, ConsoleColor.Red);
        }
        public static void Info(object value)
        {
            Write(value, ConsoleColor.DarkGray);
        }
        public static void Auth(object value)
        {
            Console.ForegroundColor = ConsoleColor.White;
            //Log("[AuthServer] " + value);
            Console.WriteLine("[AuthServer]: " + value, ConsoleColor.White);
        }
        public static void World(object value)
        {
            //Log("[WorldServer] " + value);
            Console.WriteLine("[WorldServer]: " + value, ConsoleColor.White);
        }
        public static void Log(object value)
        {
            Write(value, ConsoleColor.White);
        }
        public static void Init(object value,bool symbol = true)
        {
            Write( value, ConstantsRepertory.Symbioz_COLOR1,symbol);
        }
        public static void Init2(object value,bool symbol = true)
        {
            Write( value, ConstantsRepertory.Symbioz_COLOR2,symbol);
        }
        public static void Write(object value,ConsoleColor color,bool symbol = true)
        {
            /*Console.ForegroundColor = color;
            if (symbol)
                Console.WriteLine(LogSymbol + " " + value.ToString());
            else
                Console.WriteLine(value.ToString());*/
        }
    }
}
