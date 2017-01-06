using Symbioz.Core.Startup;
using Symbioz.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using YAXLib;

namespace Symbioz.Core
{
    public class ConfigurationManager
    {
        public const string CONFIG_NAME = "config";

        public static string ConfigurationPath = Environment.CurrentDirectory + "/" + CONFIG_NAME + ".xml";

        public static Configuration Instance = new Configuration();

        public static void Serialize()
        {
            YAXSerializer serializer = new YAXSerializer(typeof(Configuration));
            string contents = serializer.Serialize(Instance);
            File.WriteAllText(ConfigurationPath, contents);
        }
        public static void Deserialize()
        {
            YAXSerializer serializer = new YAXSerializer(typeof(Configuration));
            Instance = (Configuration)serializer.Deserialize(File.ReadAllText(ConfigurationPath));
        }
        [StartupInvoke(StartupInvokeType.Config)]
        public static void Load()
        {
            if (File.Exists(ConfigurationPath))
            {
                try
                {
                    Deserialize();
                    Logger.Log("[Config] Configuration Loaded");
                }
                catch (Exception ex)
                {
                label:
                    Logger.Error(ex.Message);
                    Logger.Init("[Config] Unable to load configuration do you want to use default configuration?");
                    Logger.Init("y/n?");
                    ConsoleKeyInfo answer = Console.ReadKey(true);
                    if (answer.Key == ConsoleKey.Y)
                    {
                        File.Delete(ConfigurationPath);
                        Load();
                        return;
                    }
                    if (answer.Key == ConsoleKey.N)
                    {
                        Environment.Exit(0);
                    }
                    goto label;
                }
            }
            else
            {
                Instance = Configuration.Default();
                Serialize();
                Logger.Log("[Config] Configuration File Created with default values");
            }

        }
    }
    [YAXComment("Symbioz Configuration")]
    public class Configuration
    {
        public int CyclicSaveInterval { get; set; }

        public string DatabaseHost { get; set; }

        public string DatabaseUser { get; set; }

        public string DatabasePassword { get; set; }

        public string DatabaseName { get; set; }

        public int ServerId { get; set; }

        public int AuthPort { get; set; }

        public int WorldPort { get; set; }

        public byte StartLevel { get; set; }

        public int StartMapId { get; set; }

        public short StartCellId { get; set; }

        public int DutyMapId { get; set; }

        public short DutyCellId { get; set; }

        public int StartKamas { get; set; }

        public string WelcomeMessage { get; set; }

        public bool ShowProtocolMessages { get; set; }

        public int KamasDropRatio { get; set; }

        public int ExperienceRatio { get; set; }

        public int ItemsDropRatio { get; set; }

        public int StartBankKamas { get; set; }

        public string Host { get; set; }

        public bool IsCustomHost { get; set; }

        public string RealHost { get; set; }

        public bool SafeRun { get; set; }

        public int CyclicFileGenerationInterval { get; set; }
        
        public string CyclicFileGenerationPath { get; set; }

        public int TimeBetweenSalesMessage { get; set; }

        public int TimeBetweenSeekMessage { get; set; }

        public int TurnBeforeFightDisconnection { get; set; }

        public int TimeBetweenCharacterSave { get; set; }

        public int TimeBetweenStart { get; set; }

        public int TimeForUseKeyring { get; set; }

        public int ArchMonsterBySubArea { get; set; }

        public int TimeBetweenSpawnArchMonster { get; set; }

        public string WelcomeSystemMessage { get; set; }

        public static Configuration Default()
        {
            Configuration config = new Configuration();
            config.CyclicSaveInterval = 80;
            config.DatabaseHost = "127.0.0.1";
            config.DatabaseUser = "root";
            config.DatabasePassword = string.Empty;
            config.DatabaseName = "Symbioz";
            config.ServerId = 30;
            config.Host = "127.0.0.1";
            config.AuthPort = 443;
            config.WorldPort = 5555;
            config.StartLevel = 1;
            config.StartMapId = 154010883;
            config.StartCellId = 383;
            config.DutyMapId = 99091983;
            config.DutyCellId = 300;
            config.StartKamas = 0;
            config.WelcomeMessage = "Symbioz " + ConstantsRepertory.VERSION;
            config.ShowProtocolMessages = true;
            config.KamasDropRatio = 1;
            config.ExperienceRatio = 1;
            config.ItemsDropRatio = 1;
            config.StartBankKamas = 0;
            config.SafeRun = false;
            config.CyclicFileGenerationInterval = 900000;
            config.CyclicFileGenerationPath = "C:/wamp/www/games/dofus2/game-export";
            config.TimeBetweenSalesMessage = 120;
            config.TimeBetweenSeekMessage = 120;
            config.TurnBeforeFightDisconnection = 20;
            config.TimeBetweenCharacterSave = 120;
            config.TimeForUseKeyring = 7200;
            config.ArchMonsterBySubArea = 1;
            config.TimeBetweenSpawnArchMonster = 1800;
            config.WelcomeSystemMessage = "Bienvenue sur notre serveur !";
            config.TimeBetweenStart = 300;
            return config;
        }
    }
}