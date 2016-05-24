using Symbioz.Core;
using Symbioz.DofusProtocol.Types;
using Symbioz.Network.Servers;
using Symbioz.ORM;
using Symbioz.Providers;
using Symbioz.World.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Symbioz.World.Records
{
    [Table("Characters", true)]
    public class CharacterRecord : ITable
    {
        static ReaderWriterLockSlim Locker = new ReaderWriterLockSlim();

        public static List<CharacterRecord> Characters = new List<CharacterRecord>();

        public static List<CharacterRecord> GetAccountCharacters(int accountid)
        {
            return Characters.FindAll(x => x.AccountId == accountid);
        }

        [Primary]
        public int Id;
        public string Name;
        public int AccountId;
        [Update]
        public string Look;
        [Update]
        public byte Level;
        public sbyte Breed;
        public bool Sex;
        [Update]
        public int MapId;
        [Update]
        public short CellId;
        [Update]
        public sbyte Direction;
        [Update]
        public int Kamas;
        [Update]
        public ulong Exp;
        [Update]
        public int TitleId;
        [Update]
        public int OrnamentId;
        [Update]
        public sbyte AlignmentSide;
        [Update]
        public sbyte AlignmentValue;
        [Update]
        public sbyte AlignmentGrade;
        [Update]
        public uint CharacterPower;
        [Update]
        public ushort StatsPoints;
        [Update]
        public ushort SpellPoints;
        [Update]
        public ushort Honor;
        [Update]
        public List<ushort> KnownTiles;
        [Update]
        public List<ushort> KnownOrnaments;
        [Update]
        public ushort ActiveTitle;
        [Update]
        public ushort ActiveOrnament;
        [Update]
        public List<byte> KnownEmotes;
        [Update]
        public int SpawnPointMapId;
        [Update]
        public short EquipedSkitterId;
        [Update]
        public List<int> KnownTips;
        [Update]
        public ushort ActualRank;
        [Update]
        public ushort BestDailyRank;
        [Update]
        public ushort MaxRank;
        [Update]
        public ushort ArenaVictoryCount;
        [Update]
        public ushort ArenaFightCount;
        [Update]
        public bool PvPEnable;

        public CharacterRecord(int id, string name, int accountid, string look, byte level, sbyte breed,
            bool sex, int mapid, short cellid, sbyte direction, int kamas, ulong exp, int titleid,
            int ornamentid, sbyte alignside, sbyte alignvalue, sbyte aligngrade, uint characterpower, ushort statspoints,
            ushort spellpoints, ushort honor, List<ushort> knowntiles, List<ushort> knownornaments, ushort activetitle,
            ushort activeornament, List<byte> knownemotes, int spawnpointmapid, short equipedskitterid, List<int> knowntips,
            ushort actualRank,ushort bestDailyRank,ushort maxRank,ushort arenaVictoryCount,ushort arenaFightsCount,bool pvpEnlable)
        {
            this.Id = id;
            this.Name = name;
            this.Level = level;
            this.AccountId = accountid;
            this.Look = look;
            this.Breed = breed;
            this.Sex = sex;
            this.MapId = mapid;
            this.CellId = cellid;
            this.Direction = direction;
            this.Kamas = kamas;
            this.Exp = exp;
            this.TitleId = titleid;
            this.OrnamentId = ornamentid;
            this.AlignmentSide = alignside;
            this.AlignmentValue = alignvalue;
            this.AlignmentGrade = aligngrade;
            this.CharacterPower = characterpower;
            this.StatsPoints = statspoints;
            this.SpellPoints = spellpoints;
            this.Honor = honor;
            this.KnownTiles = knowntiles;
            this.KnownOrnaments = knownornaments;
            this.ActiveOrnament = activeornament;
            this.ActiveTitle = activetitle;
            this.KnownEmotes = knownemotes;
            this.SpawnPointMapId = spawnpointmapid;
            this.EquipedSkitterId = equipedskitterid;
            this.KnownTips = knowntips;
            this.ActualRank = actualRank;
            this.BestDailyRank = bestDailyRank;
            this.MaxRank = maxRank;
            this.ArenaVictoryCount = arenaVictoryCount;
            this.ArenaFightCount = arenaFightsCount;
            this.PvPEnable = pvpEnlable;
        }
        [BeforeSave]
        public static void BeforeSave()
        {
            var online = WorldServer.Instance.GetAllClientsOnline();
            foreach (var client in online)
            {
                client.Character.Look.UnsetAura();
                client.Character.Record.Look = client.Character.Look.ConvertToString();
                SaveTask.UpdateElement(client.Character.Record);
                SaveTask.UpdateElement(client.Character.StatsRecord);
            }
        }
        public CharacterBaseInformations GetBaseInformation()
        {
            return new CharacterBaseInformations((uint)Id, Level, Name, ContextActorLook.Parse(Look).ToEntityLook(), Breed, Sex);
        }
        public static CharacterRecord Default(string name, int accountid, string look, sbyte breed, bool sex)
        {
            return new CharacterRecord(CharacterRecord.FindFreeId(), name, accountid, look, 1, breed, sex,
            ConfigurationManager.Instance.StartMapId, ConfigurationManager.Instance.StartCellId, 3, ConfigurationManager.Instance.StartKamas,
            1, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            new List<ushort>(), new List<ushort>(), 0, 0, new List<byte>() { 1 }, -1, 0, new List<int>(),ArenaProvider.DEFAULT_RANK,ArenaProvider.DEFAULT_RANK,
            ArenaProvider.DEFAULT_RANK,0,0,false);
        }
        public static bool CheckCharacterNameExist(string name)
        {
            var temporary = Characters.Find(x => x.Name == name);
            if (temporary == null)
                return false;
            else
                return true;
        }
        public static CharacterRecord GetCharacterRecordById(int id)
        {
            return Characters.Find(x => x.Id == id);
        }
        public static int FindFreeId()
        {
            Locker.EnterReadLock();
            try
            {
                if (Characters.Count() == 0)
                    return 1;
                var ids = Characters.ConvertAll<int>(x => x.Id);
                ids.Sort();
                return ids.Last() + 1;
            }
            finally
            {
                Locker.ExitReadLock();
            }
        }
    }
}
