﻿using Symbioz.Core;
using Symbioz.Core.Startup;
using Symbioz.DofusProtocol.Types;
using Symbioz.Network.Servers;
using Symbioz.ORM;
using Symbioz.Providers;
using Symbioz.World.Models;
using Symbioz.World.Records.Guilds;
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
        public string OldLook;
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
        [Update]
        public short Energy;
        [Update]
        public ushort DeathCount;
        [Update]
        public byte DeathMaxLevel;
        [Update]
        public string Succes;
        [Update]
        public uint CurrentLifePoint;
        [Update]
        public int LastConnection;
        [Update]
        public int MoodSmileyId;
        [Update]
        public int MerchantMode;
        [Update]
        public int PlayersKilled;
        [Ignore]
        public bool IsMerchantMode
        {
            get
            {
                return this.MerchantMode == 1 ? true : false;
            }
        }
        [Ignore]
        public string MerchantMessage;

        public CharacterRecord(int id, string name, int accountid, string look, string oldLook, byte level, sbyte breed,
            bool sex, int mapid, short cellid, sbyte direction, int kamas, ulong exp, int titleid,
            int ornamentid, sbyte alignside, sbyte alignvalue, sbyte aligngrade, uint characterpower, ushort statspoints,
            ushort spellpoints, ushort honor, List<ushort> knowntiles, List<ushort> knownornaments, ushort activetitle,
            ushort activeornament, List<byte> knownemotes, int spawnpointmapid, short equipedskitterid, List<int> knowntips,
            ushort actualRank,ushort bestDailyRank,ushort maxRank,ushort arenaVictoryCount,ushort arenaFightsCount, bool pvpEnable,
            short energy, ushort deathCount, byte deathMaxLevel, string succes, uint currentLifePoint, int lastConnection,
            int moodSmileyId, int merchantMode, int playersKilled)
        {
            this.Id = id;
            this.Name = name;
            this.Level = level;
            this.AccountId = accountid;
            this.Look = look;
            this.OldLook = oldLook;
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
            this.PvPEnable = pvpEnable;
            this.Energy = energy;
            this.DeathCount = deathCount;
            this.DeathMaxLevel = deathMaxLevel;
            this.Succes = succes;
            this.CurrentLifePoint = currentLifePoint;
            this.LastConnection = lastConnection;
            this.MoodSmileyId = moodSmileyId;
            this.MerchantMode = merchantMode;
            this.PlayersKilled = playersKilled;
        }
        [BeforeSave]
        public static void BeforeSave()
        {
            var online = WorldServer.Instance.GetAllClientsOnline();
            foreach (var client in online)
            {
                try
                {
                    client.Character.Look.UnsetAura();
                    client.Character.Record.Look = client.Character.Look.ConvertToString();
                    SaveTask.UpdateElement(client.Character.Record);
                    SaveTask.UpdateElement(client.Character.CharacterStatsRecord);
                }
                catch (Exception error)
                {
                    Logger.Error(error);
                }
            }
        }

        public CharacterBaseInformations GetBaseInformation()
        {
            return new CharacterBaseInformations((uint)Id, Level, Name, ContextActorLook.Parse(Look).ToEntityLook(), Breed, Sex);
        }
        public CharacterHardcoreOrEpicInformations GetHardcoreOrEpicInformations()
        {
            byte death = 0;

            if (this.Energy == 0)
                death = 1;
            return new CharacterHardcoreOrEpicInformations((uint)Id, Level, Name, ContextActorLook.Parse(Look).ToEntityLook(), Breed, Sex, death, this.DeathCount, this.DeathMaxLevel);
        }
        public static CharacterRecord Default(string name, int accountid, string look, sbyte breed, bool sex)
        {
            var newId = CharacterRecord.FindFreeId();
            CharacterItemRecord.RemoveAll(newId);
            GeneralShortcutRecord.RemoveAll(newId);
            SpellShortcutRecord.RemoveAll(newId);
            CharacterSpellRecord.RemoveAll(newId);
            CharacterJobRecord.RemoveAll(newId);
            BidShopGainRecord.RemoveAll(newId);
            CharacterGuildRecord.RemoveAll(newId);
            BidShopItemRecord.RemoveAll(newId);

            return new CharacterRecord(CharacterRecord.FindFreeId(), name, accountid, look, look, 1, breed, sex,
            ConfigurationManager.Instance.StartMapId, ConfigurationManager.Instance.StartCellId, 3, ConfigurationManager.Instance.StartKamas,
            1, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            new List<ushort>(), new List<ushort>(), 0, 0, new List<byte>() { 1 }, -1, 0, new List<int>(),ArenaProvider.DEFAULT_RANK,ArenaProvider.DEFAULT_RANK,
            ArenaProvider.DEFAULT_RANK,0,0,false, 10000, 0, 1, null, 0, 0, 0, 0, 0);
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

        public static CharacterRecord GetCharacterRecordByName(string name)
        {
            return Characters.Find(x => x.Name == name);
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

        public static void AddKamasOnCharacterId(int CharacterId, int Kamas)
        {
            foreach (var character in Characters)
            {
                if (character.Id == CharacterId)
                {
                    character.Kamas += Kamas;
                    SaveTask.UpdateElement(character);
                    break;
                }
            }
        }
    }
}
