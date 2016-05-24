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
        static void Logo()
        {
            Init(@"_______ __   __ _______ ______  _____  _____  ______", false);
            Init2(@"|______   \_/   |  |  | |_____]   |   |     |  ____/", false);
            Init(@"______|    |    |  |  | |_____] __|__ |_____| /_____",false);

        }
        public const string LogSymbol = "))";

        public static void OnStartup()
        {
            Console.Title = "Symbioz";
            Logo();
            Logger.Init2("Version " + ConstantsRepertory.VERSION);
            Logger.NewLine();
            Logger.Init2("Written by Skinz");
            Logger.Init2("Contributor(s): Matspyder");
            Logger.NewLine();
        }
        public static void NewLine()
        {
            Console.WriteLine(Environment.NewLine);
        }
        public static void Error(object value)
        {
            Write("Error: "+ value, ConsoleColor.Red);
        }
        public static void Info(object value)
        {
            Write(value, ConsoleColor.DarkGray);
        }
        public static void Auth(object value)
        {
            Log("[AuthServer] " + value);
        }
        public static void World(object value)
        {
            Log("[WorldServer] " + value);
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
            Console.ForegroundColor = color;
            if (symbol)
                Console.WriteLine(LogSymbol + " " + value.ToString());
            else
                Console.WriteLine(value.ToString());
        }
    }
}
