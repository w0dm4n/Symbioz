using Shader.Helper;
using Symbioz.Core.Startup;
using Symbioz.DofusProtocol.Messages;
using Symbioz.DofusProtocol.Types;
using Symbioz.Enums;
using Symbioz.Network.Clients;
using Symbioz.ORM;
using Symbioz.World.Models;
using Symbioz.World.Records.Alliances.Prisms.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Symbioz.World.Records.Alliances.Prisms
{
    [Table("Prisms")]
    public class PrismRecord : ITable
    {
        private static ReaderWriterLockSlim Locker = new ReaderWriterLockSlim();

        public static List<PrismRecord> Prisms = new List<PrismRecord>();

        [Primary]
        public int Id;
        public int AllianceId;
        public int SubAreaId;
        public int MapId;
        public int TypeId;
        public int State;
        public int StartDefenseTime;
        public int PlacementDate;
        public int RewardTokenCount;
        public int LastTimeSettingsModificationDate;
        public int LastTimeSlotModificationDate;
        public int LastTimeSlotModificationAuthorGuildId;
        public int LastTimeSlotModificationAuthorId;
        public string LastTimeSlotModificationAuthorName;
        [Ignore]
        public List<uint> ModulesItemsIds
        {
            get
            {
                List<uint> modulesItemsIds = new List<uint>();
                List<PrismModuleRecord> modulesItemsRecords = PrismModuleRecord.GetPrismModules(this.Id);
                if(modulesItemsRecords != null && modulesItemsRecords.Count > 0)
                {
                    foreach(PrismModuleRecord moduleItemRecord in modulesItemsRecords)
                    {
                        modulesItemsIds.Add((uint)moduleItemRecord.GID);
                    }
                }
                return modulesItemsIds;
            }
        }

        [Ignore]
        public MapInstance Map
        {
            get
            {
                return MapRecord.GetMap(this.MapId).Instance;
            }
        }

        [Ignore]
        public AllianceRecord Alliance
        {
            get
            {
                return AllianceRecord.GetAlliance(this.AllianceId);
            }
        }

        [Ignore]
        public int CellId;
        [Ignore]
        public sbyte Direction;

        [Ignore]
        public int NextVulnerabilityDate
        {
            get
            {
                DateTime baseNextVulnerabilityDate = DateTime.Now;
                if (baseNextVulnerabilityDate.Hour > GetKeyValuePairBySbyte(this.StartDefenseTime).Key)
                {
                    baseNextVulnerabilityDate = baseNextVulnerabilityDate.AddDays(1);
                }
                else if (baseNextVulnerabilityDate.Hour == GetKeyValuePairBySbyte(this.StartDefenseTime).Key
                    && baseNextVulnerabilityDate.Minute >= GetKeyValuePairBySbyte(this.StartDefenseTime).Value)
                {
                    baseNextVulnerabilityDate = baseNextVulnerabilityDate.AddDays(1);
                }
                return new DateTime(baseNextVulnerabilityDate.Year, baseNextVulnerabilityDate.Month, baseNextVulnerabilityDate.Day,
                    GetKeyValuePairBySbyte(this.StartDefenseTime).Key, GetKeyValuePairBySbyte(this.StartDefenseTime).Value, 0).ToEpochTime();
            }
        }

        [Ignore]
        private static Dictionary<int, KeyValuePair<int, int>> TimesBytes;

        [Ignore]
        public List<PrismModuleRecord> PrismModules { get { return PrismModuleRecord.GetPrismModules(this.Id); } }

        public bool HasTeleportationModule
        {
            get
            {
                return this.PrismModules.FirstOrDefault(x => x.GID == 14552) != null ? true : false;
            }
        }

        [Ignore]
        public PrismStateEnum ParsedState
        {
            get
            {
                if(this.State == (int)PrismStateEnum.PRISM_STATE_INVULNERABLE)
                {
                    this.CheckPrismInvulnerability();
                }
                return (PrismStateEnum)this.State;
            }
            set
            {
                this.State = (int)value;
            }
        }

        public const int ConstantNpcId = 2141;
        public const int ConstantContextualId = -10000;
        private const int BonesId = 2211;
        private const int BrightnessColor = 33157850;

        public PrismRecord(int id, int allianceId, int subAreaId, int mapId, int typeId, int state, int startDefenseTime, int placementDate,
            int rewardTokenCount, int lastTimeSettingsModificationDate, int lastTimeSlotModificationDate, int lastTimeSlotModificationAuthorGuildId, int lastTimeSlotModificationAuthorId,
            string lastTimeSlotModificationAuthorName)
        {
            this.Id = id;
            this.AllianceId = allianceId;
            this.SubAreaId = subAreaId;
            this.MapId = mapId;
            this.StartDefenseTime = startDefenseTime;
            this.PlacementDate = placementDate;
            this.RewardTokenCount = rewardTokenCount;
            this.LastTimeSettingsModificationDate = lastTimeSettingsModificationDate;
            this.LastTimeSlotModificationDate = lastTimeSlotModificationDate;
            this.LastTimeSlotModificationAuthorGuildId = lastTimeSlotModificationAuthorGuildId;
            this.LastTimeSlotModificationAuthorId = lastTimeSlotModificationAuthorId;
            this.LastTimeSlotModificationAuthorName = lastTimeSlotModificationAuthorName;
        }

        #region TimesBytes

        [StartupInvoke("TimesBytesInitialization", StartupInvokeType.Internal)]
        public static void InitiliazeTimesBytes()
        {
            TimesBytes = new Dictionary<int, KeyValuePair<int, int>>();
            DateTime baseDateTime = new DateTime(0001, 1, 1, 0, 0, 0);
            bool flag = false;
            for(int i = 0; i <= 94; i = (i + 2))
            {
                TimesBytes.Add(i, new KeyValuePair<int, int>(baseDateTime.Hour, baseDateTime.Minute));
                if(flag)
                {
                    baseDateTime = new DateTime(0001, 1, 1, baseDateTime.Hour, 0, 0);
                    baseDateTime = baseDateTime.AddHours(1);
                    flag = false;
                }
                else
                {
                    baseDateTime = baseDateTime.AddMinutes(30);
                    flag = true;
                }
            }
        }

        public static KeyValuePair<int, int> GetKeyValuePairBySbyte(int key)
        {
            return TimesBytes.FirstOrDefault(x => x.Key == key).Value;
        }

        #endregion

        #region Look

        private List<int> GetColors()
        {
            List<int> colors = new List<int>();
            colors.Add(BrightnessColor);
            ContextActorLook.AddEmptiesColors(colors, 7);
            colors.Add(this.Alliance.BackgroundColor);
            colors.Add(this.Alliance.SymbolColor);
            colors = ContextActorLook.GetDofusColors(colors);
            return colors;
        }

        public EntityLook GetLook()
        {
            return new ContextActorLook(BonesId, new List<ushort>() { (ushort)(2569 + this.Alliance.SymbolShape) },
                this.GetColors(), new List<short>(), new List<SubEntity>());
        }

        #endregion

        #region TypesInformations

        public PrismInformation GetPrismInformation()
        {
            return new PrismInformation((sbyte)this.TypeId, (sbyte)this.ParsedState, this.NextVulnerabilityDate, this.PlacementDate, (uint)this.RewardTokenCount);
        }

        public AlliancePrismInformation GetAlliancePrismInformation()
        {
            return new AlliancePrismInformation((sbyte)this.TypeId, (sbyte)this.ParsedState, this.NextVulnerabilityDate, this.PlacementDate, (uint)this.RewardTokenCount,
                this.Alliance.GetAllianceInformations());
        }

        public AllianceInsiderPrismInformation GetAllianceInsiderPrismInformation()
        {
            return new AllianceInsiderPrismInformation((sbyte)this.TypeId, (sbyte)this.ParsedState, this.NextVulnerabilityDate, this.PlacementDate, (uint)this.RewardTokenCount,
                this.LastTimeSlotModificationDate, (uint)this.LastTimeSlotModificationAuthorGuildId, (uint)this.LastTimeSlotModificationAuthorId, this.LastTimeSlotModificationAuthorName,
                this.ModulesItemsIds);
        }

        public EntityDispositionInformations GetEntityDispositionInformations()
        {
            return new EntityDispositionInformations((short)this.CellId, this.Direction);
        }

        public GameRolePlayPrismInformations GetGameRolePlayPrismInformations(WorldClient client)
        {
            GameRolePlayPrismInformations gameRolePlayPrismInformations = null;
            if (client.Character.HasAlliance && client.Character.AllianceId == this.AllianceId)
            {
                gameRolePlayPrismInformations = new GameRolePlayPrismInformations(ConstantContextualId, this.GetLook(), this.GetEntityDispositionInformations(),
                    this.GetAllianceInsiderPrismInformation());
            }
            else
            {
                gameRolePlayPrismInformations = new GameRolePlayPrismInformations(ConstantContextualId, this.GetLook(), this.GetEntityDispositionInformations(),
                    this.GetAlliancePrismInformation());
            }
            return gameRolePlayPrismInformations;
        }

        #endregion

        #region StaticFunctions

        public static PrismRecord GetPrism(int prismId)
        {
            return Prisms.FirstOrDefault(x => x.Id == prismId);
        }

        public static PrismRecord GetPrismBySubAreaId(int subAreaId)
        {
            return Prisms.FirstOrDefault(x => x.SubAreaId == subAreaId);
        }

        public static PrismRecord GetPrismByMapId(int mapId)
        {
            return Prisms.FirstOrDefault(x => x.MapId == mapId);
        }

        public static List<PrismRecord> GetAlliancePrisms(int allianceId)
        {
            List<PrismRecord> alliancePrims = new List<PrismRecord>();
            foreach (var prims in Prisms.Where(x => x.AllianceId == allianceId))
            {
                alliancePrims.Add(prims);
            }
            return alliancePrims;
        }

        #endregion

        public void RandomizeDisposition()
        {
            this.CellId = this.Map.Record.RandomWalkableCell();
            this.Direction = (sbyte)new Random().Next(0, 7);
        }

        public void RefreshOnMapInstance()
        {
            this.Map.Clients.ForEach(x => x.Send(new GameRolePlayShowActorMessage(this.GetGameRolePlayPrismInformations(x))));
        }

        public void ChangeSettings(Character character, sbyte startDefenseTime)
        {
            this.LastTimeSettingsModificationDate = DateTime.Now.ToEpochTime();
            this.StartDefenseTime = startDefenseTime;
            this.UpdateLastModificationInformations(character);
        }

        private void UpdateLastModificationInformations(Character character)
        {
            this.LastTimeSlotModificationDate = DateTime.Now.ToEpochTime();
            this.LastTimeSlotModificationAuthorGuildId = character.GuildId;
            this.LastTimeSlotModificationAuthorId = character.Id;
            this.LastTimeSlotModificationAuthorName = character.Record.Name;
        }

        #region PrismInvulnerability

        private void CheckPrismInvulnerability()
        {
            if ((this.PlacementDate.GetDateTimeFromEpoch() - DateTime.Now).TotalHours <= -1)
            {
                this.State = (int)PrismStateEnum.PRISM_STATE_NORMAL;
            }
        }

        #endregion

        #region Save()

        public void Save()
        {
            SaveTask.AddElement(this);
        }

        #endregion

        #region PopNextId()

        public static int PopNextId()
        {
            Locker.EnterReadLock();
            try
            {
                var ids = Prisms.ConvertAll<int>(x => x.Id);
                ids.Sort();
                return ids.Count == 0 ? 1 : ids.Last() + 1;
            }
            finally
            {
                Locker.ExitReadLock();
            }
        }

        #endregion
    }
}
