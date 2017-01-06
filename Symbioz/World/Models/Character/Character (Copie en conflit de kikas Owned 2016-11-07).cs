using Symbioz.Auth.Handlers;
using Symbioz.Core;
using Symbioz.DofusProtocol.Messages;
using Symbioz.DofusProtocol.Types;
using Symbioz.Enums;
using Symbioz.Network.Clients;
using Symbioz.ORM;
using Symbioz.Providers;
using Symbioz.World.Handlers;
using Symbioz.World.Models;
using Symbioz.World.Models.Alliances;
using Symbioz.World.Models.Exchanges;
using Symbioz.World.Models.Exchanges.Craft;
using Symbioz.World.Models.Fights;
using Symbioz.World.Models.Fights.Fighters;
using Symbioz.World.Models.Guilds;
using Symbioz.World.Models.Maps;
using Symbioz.World.Models.Parties;
using Symbioz.World.Records;
using Symbioz.World.Records.Alliances;
using Symbioz.World.Records.Companions;
using Symbioz.World.Records.Guilds;
using Symbioz.World.Records.Maps;
using Symbioz.World.Records.Monsters;
using Symbioz.World.Records.Friends;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Symbioz.World.Models.Succes;
using Shader.Helper;
using Symbioz.Network.Servers;
using Symbioz.Auth.Records;
using Symbioz.World.Records.Ignored;
using Symbioz.World.Records.Items;
using Symbioz.World.Records.Tracks;
using Symbioz.World.Models.Items;
using System.Timers;
using Symbioz.Auth.Models;
using Symbioz.Helper;
using Symbioz.Core.Startup;

namespace Symbioz.World.Models
{
    public class Character : IDisposable
    {
        public Account Account { get; set; }
        public int Id { get { return Record.Id; } }
        public bool IsNew = false;
        public bool isGod = false;
        public bool isDebugging = false;
        public bool GetLifePoints = false;
        public ushort SubAreaId { get; set; }
        public int LastSalesMessage = 0;
        public int LastSeekMessage = 0;
        public int LastCharacterSave = 0;
        public int LastCharacterStart = 0;
        public DateTime RegenStartTime;
        public bool IsRegeneratingLife;
        public bool CurrentlyInTrackRequest = false;
        public bool IsTracking = false;
        public short RegenRate;
        public MapRecord Map { get; set; }
        public short MovedCell { get; set; }
        public WorldClient Client { get; set; }
        public ContextActorLook Look { get; set; }
        public ContextActorLook LookSave { get; set; }
        public CharacterRecord Record { get; set; }
        public bool Busy { get { return ExchangeType != null; } }
        public Inventory Inventory { get; set; }
        public CharacterStatsRecord CharacterStatsRecord { get { return CharacterStatsRecord.CharactersStats.Find(x => x.CharacterId == Id); } }
        public void ResetCharacterStatsRecord(CharacterStatsRecord newStats ) { CharacterStatsRecord.Reset(this.CharacterStatsRecord, newStats); }
        public BasicStats CurrentStats { get; set; }
        public List<HumanOption> HumanOptions { get; set; }
        public ActorRestrictionsInformations Restrictions = CharacterHelper.GetDefaultRestrictions();
        public DialogTypeEnum? CurrentDialogType = null;
        public List<SpellItem> Spells { get { return CharacterSpellRecord.GetCharacterSpells(Id); } }
        public List<ShortcutSpell> SpellsShortcuts { get { return SpellShortcutRecord.GetCharacterShortcuts(Id); } }
        public List<Shortcut> GeneralShortcuts { get { return GeneralShortcutRecord.GetCharacterShortcuts(Id); } }
        public List<SuccesRewards> SuccesShortcuts = new List<SuccesRewards>();
        public List<FriendRecord> Friends = new List<FriendRecord>();
        public List<IgnoredRecord> Enemies = new List<IgnoredRecord>();
        public List<IgnoredRecord> Ignored = new List<IgnoredRecord>();
        public List<CharacterKeyring> Keyrings = new List<CharacterKeyring>();
        public List<CharactersMerchantsRecord> MerchantItems = new List<CharactersMerchantsRecord>();
        public PlayerTradeRequest Request
        {
            get;
            set;
        }
        public string Name { get { return Record.Name; } }
        #region Party
        public Party Party
        {
            get;
            set;
        }

        private Dictionary<uint, PartyInvitation> PartyInvitations = new Dictionary<uint, PartyInvitation>();

        public PartyInvitation GetPartyInvitation(uint partyId)
        {
            return PartyInvitations.ContainsKey(partyId) ? PartyInvitations[partyId] : null;
        }

        public bool IsInParty()
        {
            return Party != null;
        }

        public bool IsPartyLeader()
        {
            return IsInParty() && Party.Leader == this;
        }

        public void AddInvitation(PartyInvitation invitation)
        {
            PartyInvitations.Add(invitation.Party.Id, invitation);
            invitation.Party.AddGuest(this);
        }

        public void RemoveInvitation(PartyInvitation invitation)
        {
            if (PartyInvitations.ContainsValue(invitation))
                PartyInvitations.Remove(invitation.Party.Id);
        }

        public void RemoveInvitation(Party invitation)
        {
            if (PartyInvitations.ContainsKey(invitation.Id))
                PartyInvitations.Remove(invitation.Id);
        }

        public void QuitParty(bool kicked)
        {
            if (Party != null)
            {
                Party.RemoveMember(this, kicked);
                Party = null;
            }
        }

        public PartyMemberInformations GetPartyMemberInformations()
        {
            var stats = this.CharacterStatsRecord;
            int level = (int)this.Record.Level;
            int maxhp = stats.LifePoints;
            int align = (int)(sbyte)Record.AlignmentSide;
            PlayerStatus status = new PlayerStatus((sbyte)0);
            PartyCompanionMemberInformations[] memberInformationsArray = new PartyCompanionMemberInformations[0];
            return new PartyMemberInformations((uint)Id, (byte)level, Name, Look.ToEntityLook(), (sbyte)Record.Breed, Record.Sex, (uint)CurrentStats.LifePoints, (uint)maxhp, (ushort)stats.Prospecting, (byte)1, (ushort)this.Initiative, (sbyte)align, (short)0, (short)0, this.Map.Id, (ushort)this.SubAreaId, status, (IEnumerable<PartyCompanionMemberInformations>)memberInformationsArray);
        }

        public PartyInvitationMemberInformations GetPartyInvitationMemberInformations()
        {
            BasicStats current = this.CurrentStats;
            PartyCompanionMemberInformations[] memberInformationsArray = new PartyCompanionMemberInformations[0];
            return new PartyInvitationMemberInformations((uint)Id, (byte)Record.Level, Record.Name, Look.ToEntityLook(), (sbyte)Record.Breed, Record.Sex, (short)0, (short)0, this.Map.Id, (ushort)this.SubAreaId, (IEnumerable<PartyCompanionMemberInformations>)memberInformationsArray);
        }

        public PartyGuestInformations GetPartyGuestInformations(Party party)
        {
            var invitation = PartyInvitations[party.Id];
            if (invitation == null)
                return new PartyGuestInformations();
            PartyCompanionMemberInformations[] memberInformationsArray = new PartyCompanionMemberInformations[0];
            return new PartyGuestInformations(Id, (int)invitation.Party.Id, Record.Name, Look.ToEntityLook(), Record.Breed, Record.Sex, new PlayerStatus((sbyte)0), (IEnumerable<PartyCompanionMemberInformations>)memberInformationsArray);
        }
        #endregion
        #region Exchanges
        public ExchangeTypeEnum? ExchangeType = null;
        public NpcBuySellExchange NpcShopExchange { get; set; }
        public NpcTradeExchange NpcTradeExchange { get; set; }
        public SmithMagicExchange SmithMagicInstance { get; set; }
        public BankExchange BankInstance { get; set; }
        public BidShopExchange BidShopInstance { get; set; }
        public ShopStockExchange ShopStockInstance { get; set; }
        public PlayerTradeExchange PlayerTradeInstance { get; set; }
        public CraftExchange CraftInstance { get; set; }
        public PrismExchange PrismStorageInstance { get; set; }
        public GuildInvitationDialog GuildInvitationDialog { get; set; }
        public AllianceInvitationDialog AllianceInvitationDialog { get; set; }
        public PlayerTrader Trader;
        #endregion

        public void ResetTrade()
        {
            PlayerTradeInstance = null;
            Trader = null;
        }

        public bool IsFighting { get { return !FighterInstance.IsNull(); } }

        public bool SearchingArena { get { return ArenaProvider.Instance.IsSearching(Client); } }
        public int GuildId { get { return CharacterGuildRecord.GetCharacterGuild(Id).GuildId; } }
        public int AllianceId { get { return GuildAllianceRecord.GetCharacterAlliance(GuildId).AllianceId; } }

        public CharacterFighter FighterInstance { get; set; }
        public bool CancelMonsterAgression = false;

        public CompanionRecord EquipedCompanion { get; set; }

        public int Initiative
        {
            get
            {
                return (int)(CharacterStatsRecord.Strength + CharacterStatsRecord.Intelligence + CharacterStatsRecord.Agility +
                    CharacterStatsRecord.Chance + CharacterStatsRecord.Initiative * (CurrentStats.LifePoints / CharacterStatsRecord.LifePoints));
            }
        }

        public PlayerStatus PlayerStatus;

        public Character(CharacterRecord record, WorldClient client)
        {
            this.Client = client;
            this.Record = record;

                this.Account = AccountsProvider.GetAccountFromDb(this.Record.AccountId);
                AccountsProvider.UpdateAccountsOnlineState(this.Account.Id, true);

                this.Look = ContextActorLook.Parse(Record.Look);
                this.HumanOptions = new List<HumanOption>();

                this.Inventory = new Inventory(this);

                this.LoadMerchantItems();
                this.LoadFriends();
                this.LoadEnemies();

                this.PlayerStatus = new PlayerStatus((sbyte)PlayerStatusEnum.PLAYER_STATUS_AVAILABLE);
        }
        public void OnConnectedBasicActions()
        {
            if (this.Record.MerchantMode == 1)
            {
                this.Inventory.Refresh();
                this.UnSetMerchantLook();
                this.Record.MerchantMode = 0;
            }
            this.SendOnlineFriendsCountMessage();
            this.GetLifeBackAtConnection();
            this.RegenLife(10);
        }
        public void OnConnectedGuildInformations()
        {
            if (this.HasGuild)
            {
                Client.Send(new GuildMembershipMessage(this.GetGuild().GetGuildInformations(), CharacterGuildRecord.GetCharacterGuild(this.Id).Rights, true));
                this.HumanOptions.Add(new HumanOptionGuild(this.GetGuild().GetGuildInformations()));
                this.SetGuildLook();
                Client.Character.RefreshOnMapInstance();
            }
        }
        public void OnConnectedAllianceInformations()
        {
            if (this.HasAlliance)
            {
                Client.Send(new AllianceMembershipMessage(GetAlliance().GetAllianceInformations(), true));
                this.HumanOptions.Add(new HumanOptionAlliance(GetAlliance().GetAllianceInformations(), (sbyte)0));
                Client.Send(AllianceProvider.GetAllianceInsiderInfoMessage(this.GetAlliance()));
                this.SetAllianceAndGuildLook();
                Client.Character.RefreshOnMapInstance();
            }
        }
        public void SetGuildLook()
        {
            List<int> lookIndexedColors = new List<int>();
            lookIndexedColors.AddRange(this.GetCharacterBaseIndexedColors());
            lookIndexedColors.Add(ContextActorLook.EMPTY_COLOR);
            lookIndexedColors.Add(this.GetGuild().BackgroundColor);
            lookIndexedColors.Add(this.GetGuild().SymbolColor);
            lookIndexedColors = ContextActorLook.GetDofusColors(lookIndexedColors);

            this.Look = new ContextActorLook(this.Look.bonesId, this.Look.skins, lookIndexedColors, this.Look.scales, this.Look.subentities);
        }
        public void SetAllianceAndGuildLook()
        {
            List<int> lookIndexedColors = new List<int>();
            lookIndexedColors.AddRange(this.GetCharacterBaseIndexedColors());
            lookIndexedColors.Add(ContextActorLook.EMPTY_COLOR);
            lookIndexedColors.Add(this.GetGuild().BackgroundColor);
            lookIndexedColors.Add(this.GetGuild().SymbolColor);
            lookIndexedColors.Add(this.GetAlliance().BackgroundColor);
            lookIndexedColors.Add(this.GetAlliance().SymbolColor);

            lookIndexedColors = ContextActorLook.GetDofusColors(lookIndexedColors);

            this.Look = new ContextActorLook(this.Look.bonesId, this.Look.skins, lookIndexedColors, this.Look.scales, this.Look.subentities);
        }
        public List<int> GetCharacterBaseIndexedColors()
        {
            var Color = this.Look.indexedColors;
            var Empty = new List<int>();
            if (Color.Count >= 5)
                return this.Look.indexedColors.GetRange(0, 5);
            else
            {
                Empty.Add(0);
                Empty.Add(0);
                Empty.Add(0);
                Empty.Add(0);
                Empty.Add(0);
                return Empty;
            }
        }
        public CharacterFighter CreateFighter(FightTeam team, Fight currentFight = null)
        {
            Look.UnsetAura();
            RefreshOnMapInstance();
            Map.Instance.RemoveClient(Client);
            Client.Send(new GameContextDestroyMessage());
            Client.Send(new GameContextCreateMessage(2));
            Client.Send(new GameFightStartingMessage((sbyte)team.Fight.FightType, 0, 1));
            var fighter = new CharacterFighter(Client, team);
            FighterInstance = fighter;
            if (EquipedCompanion != null)
            {
                if (currentFight != null && (currentFight is FightPvM || currentFight is FightDual))
                {
                    var cfighter = new CompanionFighter(EquipedCompanion, fighter, team);
                    team.AddFighter(cfighter);
                }
            }
            return fighter;
        }
        public void ReCreateFighter(FightTeam team)
        {
            Client.Send(new GameFightStartingMessage((sbyte)team.Fight.FightType, 0, 1));
        }
        public void SendMap(Message message)
        {
            if (Map != null && Map.Instance != null)
                Map.Instance.Send(message);
        }
        public void OnConnectedNotifications()
        {
            this.SendWarnOnStateMessages();
            Client.Send(new TextInformationMessage((sbyte)TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 89, new string[0]));
            Client.Character.Reply(ConfigurationManager.Instance.WelcomeMessage, System.Drawing.Color.GhostWhite);
            BidShopsHandler.AddEventualBidShopGains(Client);
            if (this.HasGuild)
            {
                if (!string.IsNullOrEmpty(this.GetGuild().GuildWelcomeMessage))
                {
                    this.Reply(string.Format("(Annonce) <b>{0}</b> : {1}", this.GetGuild().Name, this.GetGuild().GuildWelcomeMessage), Color.Purple);
                }
            }
            if (this.HasAlliance)
            {
                if (!string.IsNullOrEmpty(this.GetAlliance().AllianceWelcomeMessage))
                {
                    this.Reply(string.Format("(Annonce) <b>{0}</b> : {1}", this.GetAlliance().Name, this.GetAlliance().AllianceWelcomeMessage), Color.Pink);
                }
            }
        }
        public void RefreshArenasInfos()
        {
            Client.Send(new GameRolePlayArenaUpdatePlayerInfosMessage(Record.ActualRank, Record.BestDailyRank, Record.MaxRank, Record.ArenaVictoryCount, Record.ArenaFightCount));
        }
        public void TeleportToSpawnPoint()
        {
            if (Record.SpawnPointMapId != -1)
            {
                InteractiveRecord zaap = MapRecord.GetMap(Record.SpawnPointMapId).Zaap;
                Teleport(Record.SpawnPointMapId, (short)InteractiveRecord.GetTeleporterCellId(zaap.MapId, TeleporterTypeEnum.TELEPORTER_ZAAP));
            }
            else
            {
                Client.Character.Teleport(ConfigurationManager.Instance.StartMapId, ConfigurationManager.Instance.StartCellId);
            }
            Reply("Vous avez été téléporté.");
        }
        public void Teleport(int mapid)
        {
            Client.Character.Teleport(mapid, Record.CellId);
        }
        public void Teleport(int mapid, short cellid)
        {
            if (cellid < 0 || cellid > 560)
                cellid = 350;
            if (IsFighting)
                return;
            if (Map != null && Map.Id == mapid)
            {
                if (cellid >= 0)
                {
                    SendMap(new TeleportOnSameMapMessage(Id, (ushort)cellid));
                    Record.CellId = cellid;
                }

                else
                    SendMap(new TeleportOnSameMapMessage(Id, (ushort)Client.Character.Record.CellId));
                return;
            }
            if (Map != null)
                Map.Instance.RemoveClient(Client);

            MapsHandler.SendCurrentMapMessage(Client, mapid);
            if (cellid != -1)
                this.Record.CellId = cellid;
            this.Record.MapId = mapid;
       
        }
        public void AddOrnament(ushort id)
        {
            if (Record.KnownOrnaments.Contains(id))
            {
                Reply("Vous possedez déja cet ornament.");
                return;
            }
            Record.KnownOrnaments.Add(id);
            Client.Send(new OrnamentGainedMessage((short)id));
        }
        public void AddTitle(ushort id)
        {
            if (Record.KnownTiles.Contains(id))
            {
                Reply("Vous possedez déja ce titre.");
                return;
            }
            Record.KnownTiles.Add(id);
            Client.Send(new TitleGainedMessage(id));
        }
        public bool LearnSpell(ushort spellid)
        {
            if (Spells.Contains<SpellItem>(x => x.spellId == spellid))
            {
                Client.Character.Reply("Vous connaissez déja ce sort.");
                return false;
            }
            new CharacterSpellRecord(CharacterSpellRecord.CharactersSpells.PopNextId<CharacterSpellRecord>(x => x.Id), Id, spellid, 1).AddElement();
            new SpellShortcutRecord(SpellShortcutRecord.SpellsShortcuts.PopNextId<SpellShortcutRecord>(x => x.Id), Id, spellid, SpellShortcutRecord.GetFreeSlotId(Id)).AddElement();
            RefreshShortcuts();
            RefreshSpells();
            Client.Character.Reply("Vous avez appris un nouveau sort!");
            return true;
        }
        public SpellItem GetSpell(ushort id)
        {
            return Spells.Find(x => x.spellId == id);
        }
        sbyte GetBoostCost(sbyte actualspellgrade, sbyte newgrade)
        {
            sbyte cost = 0;
            for (sbyte i = actualspellgrade; i < newgrade; i++)
            {
                cost += i;
            }
            return (sbyte)(cost);
        }
        public ActorAlignmentInformations GetActorAlignement()
        {
            return new ActorAlignmentInformations(Record.AlignmentSide,
                (sbyte)(Record.PvPEnable ? Record.AlignmentValue : 0), (sbyte)(Record.PvPEnable ? Record.AlignmentGrade : 0), Record.CharacterPower);
        }
        public ActorExtendedAlignmentInformations GetActorExtendedAlignement()
        {
            /*AggressableStatusEnum agressableStatus = Record.PvPEnable ? AggressableStatusEnum.PvP_ENABLED_AGGRESSABLE : AggressableStatusEnum.NON_AGGRESSABLE;
             var align = new ActorExtendedAlignmentInformations(Record.AlignmentSide, (sbyte)(Record.PvPEnable ? Record.AlignmentValue : 0), (sbyte)(Record.PvPEnable ? Record.AlignmentGrade : 0), Record.CharacterPower,
                Record.Honor, ExperienceRecord.GetHonorForGrade(Record.AlignmentGrade), ExperienceRecord.GetHonorForGrade((sbyte)(Record.AlignmentGrade + 1)), (sbyte)agressableStatus);
             return align;*/
            var align = new ActorExtendedAlignmentInformations(1, 1, 1, 0, 1, 1, 1, (sbyte)AggressableStatusEnum.AvA_ENABLED_AGGRESSABLE);
            return (align);
        }
        public void TooglePvPMode(bool state)
        {
            this.Record.PvPEnable = state;
            this.RefreshStats();
            this.RefreshOnMapInstance();

        }
        public void SetAlign(AlignmentSideEnum align)
        {
            Record.AlignmentSide = (sbyte)align;
            Record.CharacterPower = 0;
            Record.AlignmentValue = 0;
            Record.AlignmentGrade = 1;
            Record.Honor = 0;
            Client.Character.Reply("Vous avez changé d'alignement (" + align + ")");
            RefreshStats();
            RefreshOnMapInstance();

        }
        public void BoostSpell(ushort spellid, sbyte level)
        {
            SpellItem actualSpell = GetSpell(spellid);
            sbyte cost = GetBoostCost(actualSpell.spellLevel, level);
            Record.SpellPoints -= (ushort)cost;
            actualSpell.spellLevel = level;
            CharacterSpellRecord.UpdateSpellLevel(actualSpell);
            Client.Send(new SpellUpgradeSuccessMessage(spellid, level));
            RefreshStats();
        }
        public void SelectOrnament(ushort id)
        {
            HumanOptions.RemoveAll(x => x is HumanOptionOrnament);
            if (id != 0)
                HumanOptions.Add(new HumanOptionOrnament(id));
            Record.ActiveOrnament = id;
            RefreshOnMapInstance();
        }
        public void SelectTitle(ushort id)
        {
            HumanOptions.RemoveAll(x => x is HumanOptionTitle);
            if (id != 0)
                HumanOptions.Add(new HumanOptionTitle(id, "Symbioz"));
            Record.ActiveTitle = id;
            RefreshOnMapInstance();
        }
        public void LearnJob(sbyte jobid)
        {
            new CharacterJobRecord(Id, jobid, 1, 0).AddElement();
            RefreshJobs();
        }
        public void UpdateBreedSpells(bool sendpackets = true)
        {
            foreach (var spell in BreedSpellRecord.GetBreedSpellsForLevel(Record.Level, Record.Breed, Spells.ConvertAll<short>(x => (short)x.spellId)))
            {
                var spellRecord = new CharacterSpellRecord(CharacterSpellRecord.CharactersSpells.PopNextId<CharacterSpellRecord>(x => x.Id), Id, spell.spellId, 1);
                var shortcutRecord = new SpellShortcutRecord(SpellShortcutRecord.SpellsShortcuts.PopNextId<SpellShortcutRecord>(x => x.Id), Id, (ushort)spell.spellId, SpellShortcutRecord.GetFreeSlotId(Id));
                SaveTask.AddElement(spellRecord, this.Id);
                SaveTask.AddElement(shortcutRecord, this.Id);
            }
            if (sendpackets)
            {
                RefreshShortcuts();
                RefreshSpells();
            }
        }
        public void RefreshShortcuts()
        {
            Client.Send(new ShortcutBarContentMessage((sbyte)ShortcutBarEnum.GENERAL_SHORTCUT_BAR, GeneralShortcuts));
            Client.Send(new ShortcutBarContentMessage((sbyte)ShortcutBarEnum.SPELL_SHORTCUT_BAR, SpellsShortcuts));
        }
        public void RefreshSpells()
        {
            Client.Send(new SpellListMessage(true, Spells));
        }
        public void LearnAllJobs()
        {
            foreach (var job in JobRecord.Jobs)
            {
                LearnJob(job.Id);
            }
        }
        public void LearnAllEmotes()
        {
            foreach (EmoteRecord emote in EmoteRecord.Emotes)
            {
                /* if(ConfigurationManager.Instance.UnauthorizedCreationGiveEmotes.Contains(emote.Id) == false)
                 {
                     LearnEmote((byte)(emote.Id));
                 }*/
            }
        }
        public void ForgetAllEmotes()
        {
            foreach (var emote in EmoteRecord.Emotes)
            {
                ForgetEmote(emote.Id);
            }
        }
        public void RefreshJobs()
        {
            var recordDatas = CharacterJobRecord.GetCharacterJobsDatas(Id);
            Client.Send(new JobCrafterDirectorySettingsMessage(recordDatas.JobSettings));
            Client.Send(new JobDescriptionMessage(recordDatas.JobsDescriptions));
            Client.Send(new JobExperienceMultiUpdateMessage(recordDatas.JobsExperiences));
        }
        public void SetLevel(uint level)
        {
            if (level <= Record.Level)
            {
                Client.Character.NotificationError("Le niveau doit être plus haut que " + Record.Level);
                return;
            }
            AddXp(ExperienceRecord.GetExperienceForLevel(level) - Record.Exp, false);
            RefreshEmotes();
            RefreshStats();
            RefreshSpells();
            RefreshShortcuts();
            Client.Send(new CharacterLevelUpMessage(Record.Level));
            if (this.Party != null)
            {
                foreach (Character c in this.Party.Members)
                {
                    c.Client.Send(new PartyUpdateMessage((uint)this.Party.Id,
                        this.GetPartyMemberInformations()));
                }
            }
            if (level >= 100 && !this.Record.KnownOrnaments.Contains(13))
            {
                this.AddOrnament(13);
            }
            if (level >= 160 && !this.Record.KnownOrnaments.Contains(14))
            {
                this.AddOrnament(14);
            }
            if (level >= 200 && !this.Record.KnownOrnaments.Contains(15))
            {
                this.AddOrnament(15);
            }
        }
        public void AddXp(ulong amount, bool sendpackets = true)
        {
            if (Record.Level == 200)
                return;
            var exp = ExperienceRecord.GetExperienceForLevel((uint)Record.Level + 1);
            if (Record.Exp + amount >= exp)
            {
                Record.Level++;
                this.GetLifePoints = true;
                if (Record.Level == 100)
                {
                    CharacterStatsRecord.ActionPoints++;
                    LearnEmote(22, sendpackets);
                }
                CharacterStatsRecord.LifePoints += 5;
                CurrentStats.LifePoints += 5;
                Record.CurrentLifePoint = CurrentStats.LifePoints;
                Record.SpellPoints += 1;
                Record.StatsPoints += 5;
                UpdateBreedSpells(sendpackets);
                if (sendpackets)
                {
                    Client.Character.SendMap(new CharacterLevelUpInformationMessage(Record.Level, Record.Name, (uint)Id));
                    Client.Send(new CharacterLevelUpMessage(Record.Level));
                }
                if (Record.Level == 200)
                {
                    Record.Exp = exp;
                    if (sendpackets)
                        RefreshStats();
                    return;
                }
                AddXp(amount, sendpackets);
            }
            else
                Record.Exp += amount;
            if (sendpackets)
                RefreshStats();
        }
        public void AddHonor(ushort amount)
        {
            if (Record.AlignmentGrade == 10)
                return;
            var exp = ExperienceRecord.GetHonorForGrade((sbyte)(Record.AlignmentGrade + 1));
            if (Record.Honor + amount >= exp)
            {
                Record.AlignmentGrade++;

                Client.Character.Reply("Vous venez de passer grade " + Record.AlignmentGrade + ".");

                if (Record.AlignmentGrade == 10)
                {
                    Record.Honor = exp;
                    RefreshStats();
                    RefreshOnMapInstance();
                    return;
                }
                AddHonor(amount);

            }
            else
                Record.Honor += amount;

            RefreshStats();
            RefreshOnMapInstance();

        }
        public void AddJobXp(sbyte jobid, ulong amount)
        {

            var job = CharacterJobRecord.GetJob(Id, jobid);
            if (job.JobLevel == 200)
                return;
            var exp = ExperienceRecord.GetExperienceForLevel((uint)(job.JobLevel + 1));
            if (job.JobExp + amount >= exp)
            {
                job.JobLevel++;
                Client.Send(new JobLevelUpMessage(job.JobLevel, job.GetJobDescription()));
                if (job.JobLevel == 200)
                {
                    job.JobExp = exp;
                    RefreshJobs();
                    return;
                }
                AddJobXp(jobid, amount);

            }
            else
                job.JobExp += amount;
            SaveTask.UpdateElement(job);
            RefreshJobs();
        }
        public void OpenPaddock()
        {
            CurrentDialogType = DialogTypeEnum.DIALOG_EXCHANGE;
            Client.Send(new PaddockPropertiesMessage(new PaddockInformations(10, 10)));
            Client.Send(new ExchangeStartOkMountMessage(new List<MountClientData>(), new List<MountClientData>()));
        }
        public void PlayAura(byte emoteid)
        {
            if (IsFighting)
                return;
            ushort bonesId = 0;
            switch (emoteid)
            {
                case 22:
                    if (Record.Level >= 100 && Record.Level != 200)
                        bonesId = 169;
                    else
                        bonesId = 170;
                    break;
                case 55:
                    bonesId = 1501;
                    break;
                case 23:
                    bonesId = 171;
                    break;
                case 40:
                    bonesId = 1465;
                    break;
            }
            Client.Character.Look.SetAura(bonesId);
            RefreshOnMapInstance();
        }
        public void InitializeCosmetics()
        {
            if (Record.ActiveTitle != 0)
                HumanOptions.Add(new HumanOptionTitle(Record.ActiveTitle, "Symbioz"));
            if (Record.ActiveOrnament != 0)
                HumanOptions.Add(new HumanOptionOrnament(Record.ActiveOrnament));
        }
        public void RefreshMap()
        {
            this.Client.Send(new GameRolePlayShowActorMessage(GetRolePlayActorInformations()));
        }
        public void CheckMapTip(int mapid)
        {
            MapTipRecord tip = MapTipRecord.GetMapTip(mapid);
            if (tip != null && !Record.KnownTips.Contains(tip.MapId))
            {
                ShowNotification(tip.Tip);
                Record.KnownTips.Add(tip.MapId);
            }
        }
        public void NotificationError(string message)
        {
            Client.Send(new NotificationByServerMessage(30, new string[] { message }, true));
        }
        public void ShowNotification(string message)
        {
            Client.Send(new NotificationByServerMessage(24, new string[] { message }, true));
        }
        public void RefreshEmotes()
        {
            Client.Send(new EmoteListMessage(Record.KnownEmotes));
        }
        public bool LearnEmote(byte id, bool refresh = true)
        {
            var template = EmoteRecord.GetEmote(id);
            if (!Record.KnownEmotes.Contains(id))
            {
                Record.KnownEmotes.Add(id);
                Client.Send(new EmoteAddMessage(id));
                return true;
            }
            else
            {
                Reply("Vous connaissez déja cette émote (" + template.Name + ").");
                return false;
            }
        }
        public void ForgetEmote(byte id)
        {
            Record.KnownEmotes.Remove(id);
            Client.Send(new EmoteRemoveMessage(id));
        }
        public void RejoinMap(bool spawnjoin, bool winner)
        {
            Client.Send(new GameContextDestroyMessage());
            Client.Send(new GameContextCreateMessage(1));
            if (FighterInstance != null && FighterInstance.Fight != null && FighterInstance.Fight.Map != null && FighterInstance.Fight.Map.DugeonMap != false)
            {
                if (winner && FighterInstance.Fight.Map.DugeonMap && FighterInstance.Fight is FightPvM)
                {
                    var dungeonMap = DungeonRecord.GetDungeonData(FighterInstance.Fight.Map.Id);
                    FighterInstance = null;
                    Client.Character.Teleport(dungeonMap.TeleportMapId, dungeonMap.TeleportCellId);
                    RefreshStats();
                    return;
                }
            }
            FighterInstance = null;
            if (spawnjoin && !winner)
            {
                Client.Character.Teleport(Client.Character.Record.MapId, Client.Character.Record.CellId);
                /*if (Client.Character.Record.SpawnPointMapId != -1)
                {
                    MapsHandler.SendCurrentMapMessage(Client, Client.Character.Record.SpawnPointMapId);
                    Record.CellId = (short)InteractiveRecord.GetTeleporterCellId(Client.Character.Record.SpawnPointMapId, TeleporterTypeEnum.TELEPORTER_ZAAP);
                }
                else
                    Client.Character.Teleport(ConfigurationManager.Instance.StartMapId, ConfigurationManager.Instance.StartCellId);*/
            }
            else
                MapsHandler.SendCurrentMapMessage(Client, Client.Character.Record.MapId);
            if (Client.Character.Restrictions.isDead == false)
                Client.Character.RegenLife(10);
            Client.Character.RefreshStats();
        }
        public void EquipCompanion(short templateid)
        {
            CompanionRecord companion = CompanionRecord.GetCompanion(templateid);
            this.EquipedCompanion = companion;
        }
        public void UnequipCompanion()
        {
            this.EquipedCompanion = null;
        }
        public void RefreshOnMapInstance()
        {
            SendMap(new GameRolePlayShowActorMessage(GetRolePlayActorInformations()));
        }
        public void RefreshPvPInfos()
        {
            Client.Send(new AlignmentRankUpdateMessage(1, false));

        }
        public void DisplaySmiley(sbyte smileyid)
        {
            SendMap(new ChatSmileyMessage(Id, smileyid, Client.Account.Id));
        }
        public GameRolePlayCharacterInformations GetRolePlayActorInformations()
        {
            return new GameRolePlayCharacterInformations(Id, Look.ToEntityLook(),
                new EntityDispositionInformations(Record.CellId, Record.Direction),
                Record.Name, new HumanInformations(Restrictions, Record.Sex, HumanOptions),
                Client.Account.Id, GetActorAlignement());
        }

        public void SetMerchantLook()
        {
            this.Look.subentities.Add(new SubEntity((sbyte)SubEntityBindingPointCategoryEnum.HOOK_POINT_CATEGORY_MERCHANT_BAG, 0, new ContextActorLook(237, new List<ushort>(), new List<int>(), new List<short>() { 100 }, new List<SubEntity>())));
            this.Look.UnsetAura();
        }

        public void UnSetMerchantLook()
        {
            foreach (var subs in this.Look.subentities)
            {
                if (subs.bindingPointCategory == (sbyte)SubEntityBindingPointCategoryEnum.HOOK_POINT_CATEGORY_MERCHANT_BAG)
                {
                    this.Look.subentities.Remove(subs);
                    break;
                }
            }
        }

        public GameRolePlayMerchantInformations GetRolePlayMerchantInformations()
        {
             return new GameRolePlayMerchantInformations(Id, this.Look.Clone(), new EntityDispositionInformations(this.Record.CellId, this.Record.Direction), this.Record.Name
                , 3, this.HumanOptions);
        }

        public void AddKamas(int amount, bool shownotif = false)
        {
            if (amount <= int.MaxValue)
            {
                Record.Kamas += amount;
                Inventory.Refresh();
            }
            if (shownotif)
                Reply("Vous avez obtenu " + amount + " kama(s).");
        }
        public bool RemoveKamas(int amount, bool shownotif = false)
        {
            if (Record.Kamas >= amount)
            {
                Record.Kamas -= amount;
                Inventory.Refresh();
                if (shownotif)
                    Reply("Vous avez perdu " + amount + " kama(s).");
                return true;
            }
            else
            {
                Reply("Vous ne possédez pas assez de kamas");
                return false;
            }


        }
        public static List<Character> Convert(IEnumerable<CharacterRecord> records, WorldClient client)
        {
            return records.ToList().ConvertAll<Character>(x => new Character(x, client));
        }
        public void Reply(object value, Color color, bool bold = false, bool underline = false)
        {
            value = ApplyPolice(value, bold, underline);
            Client.Send(new TextInformationMessage(0, 0, new string[] { string.Format("<font color=\"#{0}\">{1}</font>", color.ToArgb().ToString("X"), value) }));
        }
        object ApplyPolice(object value, bool bold, bool underline)
        {
            if (bold)
                value = "<b>" + value + "</b>";
            if (underline)
                value = "<u>" + value + "</u>";
            return value;
        }
        public void Reply(object value, bool bold = false, bool underline = false)
        {
            value = ApplyPolice(value, bold, underline);
            Client.Send(new TextInformationMessage(0, 0, new string[] { value.ToString() }));
        }
        public void ReplyImportant(object value)
        {
            Reply(value, Color.DarkOrange, false, false);
        }
        public void ReplyError(object value)
        {
            Reply("[Erreur] " + value, Color.Red, false, true);
        }

        public void ReplyInConsole(string content, ConsoleMessageTypeEnum type = ConsoleMessageTypeEnum.CONSOLE_TEXT_MESSAGE)
        {
            this.Client.Send(new ConsoleMessage((sbyte)type, content));
        }
        /// <summary>
        /// CharacterCharacteristicsInformations
        /// </summary>
        public void RefreshStats()
        {
            Client.Send(new CharacterStatsListMessage(CharacterStatsRecord.GetCharacterCharacteristics(CharacterStatsRecord, this)));
        }
        public void LeaveDialog()
        {
            ContextRolePlayHandler.HandleLeaveDialog(null, Client);
        }
        public void LeaveExchange()
        {
            switch (ExchangeType)
            {
                case ExchangeTypeEnum.CRAFT:
                    if (CraftInstance != null)
                        CraftInstance.CancelExchange();
                    break;
                case ExchangeTypeEnum.BIDHOUSE_SELL:
                    if (BidShopInstance != null)
                        BidShopInstance.CancelExchange();
                    break;
                case ExchangeTypeEnum.BIDHOUSE_BUY:
                    if (BidShopInstance != null)
                        BidShopInstance.CancelExchange();
                    break;
                case ExchangeTypeEnum.STORAGE:
                    if (BankInstance != null)
                        BankInstance.CancelExchange();
                    if (ShopStockInstance != null)
                        ShopStockInstance.CancelExchange();
                    break;
                case ExchangeTypeEnum.RUNES_TRADE:
                    if (SmithMagicInstance != null)
                        SmithMagicInstance.CancelExchange();
                    break;
                case ExchangeTypeEnum.NPC_SHOP:
                    if (NpcShopExchange != null)
                        NpcShopExchange.CancelExchange();
                    break;
                case ExchangeTypeEnum.NPC_TRADE:
                    if (NpcTradeExchange != null)
                        NpcTradeExchange.CancelExchange();
                    break;
            }
            if (PlayerTradeInstance != null)
                PlayerTradeInstance.CancelExchange();
            Client.Send(new ExchangeLeaveMessage((sbyte)DialogTypeEnum.DIALOG_EXCHANGE, true));
            ExchangeType = null;
        }

        public void Dispose()
        {
            if (this != null)
            {
                if (Map != null && this.Record.MerchantMode == 0)
                    Map.Instance.RemoveClient(Client);
                if (SearchingArena)
                    ArenaProvider.Instance.OnClientDisconnected(Client);
                if (IsFighting)
                    FighterInstance.OnDisconnect();
                if (PlayerTradeInstance != null)
                    PlayerTradeInstance.CancelExchange();
                if (Party != null)
                    QuitParty(false);
                foreach (var partyInvitation in PartyInvitations.Values)
                {
                    partyInvitation.Refuse();
                }
                Client.Character.Look.UnsetAura();
                Record.Look = Look.ConvertToString();
                SaveTask.UpdateElement(Record, this.Id);
                SaveTask.UpdateElement(CharacterStatsRecord, this.Id);
                Inventory.SaveItems();
            }
        }

        public void Save()
        {
            SaveTask.UpdateElement(Record, this.Id);
            SaveTask.UpdateElement(CharacterStatsRecord, this.Id);
            Inventory.SaveItems();
        }

    

        public GuildRecord GetGuild()
        {
            return HasGuild ? GuildRecord.GetGuild(GuildId) : null;
        }

        public bool HasGuild { get { return GuildProvider.Instance.HasGuild(Id); } }

        public void SendGuildInfos()
        {
            if (HasGuild)
            {
                Client.Send(new GuildJoinedMessage(GuildRecord.GetGuild(GuildId).GetGuildInformations(), CharacterGuildRecord.GetCharacterGuild(Id).Rights, false));
                HumanOptions.Add(new HumanOptionGuild(GetGuild().GetGuildInformations()));
            }
        }

        public AllianceRecord GetAlliance()
        {
            return HasAlliance ? AllianceRecord.GetAlliance(AllianceId) : null;
        }

        public bool HasAlliance
        {
            get
            {
                if (HasGuild)
                {
                    if (GuildAllianceRecord.GuildsAlliances.Find(x => x.GuildId == GetGuild().Id) != null)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public CharacterMinimalInformations GetCharacterMinimalInformations()
        {
            return new CharacterMinimalInformations((uint)this.Id, (byte)this.Record.Level, this.Record.Name);
        }

        public CharacterMinimalPlusLookInformations GetCharacterMinimalPlusLookInformations()
        {
            return new CharacterMinimalPlusLookInformations((uint)this.Id, (byte)this.Record.Level, this.Record.Name, this.Look);
        }

        public void SendMessage(string message)
        {
            Client.Send(new TextInformationMessage((sbyte)TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 0, new string[] { message }));
        }

        public List<string> GetSuccess()
        {
            if (Record.Succes == null)
            {
                return new List<string>();
            }
            else

                return Record.Succes.Split(',').ToList();
        }

        public bool HasSucces(int Id)
        {
            if (GetSuccess().Contains(Id.ToString()))
                return true;
            else
                return false;
        }

        public void AddAchievements(string id)
        {
            if (Record.Succes == null)
                Record.Succes = id;
            else
                Record.Succes += "," + id;
        }

        public void RegenLife(short RegenRate)
        {
            if (this.CurrentStats.LifePoints < this.CharacterStatsRecord.LifePoints)
            {
                this.Client.Send(new LifePointsRegenBeginMessage((byte)RegenRate));
                RegenStartTime = DateTime.Now;

                this.RegenRate = RegenRate;
                this.IsRegeneratingLife = true;
            }
        }

        public void CheckRegen()
        {
            if (this.IsRegeneratingLife)
            {
                if (this.RegenRate == 3)
                {
                    if (this.CurrentStats.LifePoints < this.CharacterStatsRecord.LifePoints)
                    {
                        this.StopRegenLife();
                        this.RegenLife(10);
                    }
                }
            }
        }

        public void GetLifeBackAtConnection()
        {
            var StartTime = this.Record.LastConnection;
            var AtBegin = this.CurrentStats.LifePoints;
            if (StartTime == 0)
                return;
            var CurrentTime = DateTimeUtils.GetEpochFromDateTime(DateTime.Now);
            var LifePointsToAdd = 0;
            while (StartTime < CurrentTime)
            {
                if ((this.CurrentStats.LifePoints + LifePointsToAdd) >= this.CharacterStatsRecord.LifePoints)
                    break;
                LifePointsToAdd++;
                StartTime++;
            }
            //LifePointsToAdd = LifePointsToAdd / 2; If we wanna reduce the rate of life back at the reconnection
            this.CurrentStats.LifePoints += (uint)LifePointsToAdd;
            if (this.CurrentStats.LifePoints > this.CharacterStatsRecord.LifePoints)
                this.CurrentStats.LifePoints = (uint)this.CharacterStatsRecord.LifePoints;
            this.Record.CurrentLifePoint = this.CurrentStats.LifePoints;
            if (LifePointsToAdd != 0 && this.CurrentStats.LifePoints != AtBegin)
            {
                Client.Character.Reply("Vous avez récupéré <b>" + LifePointsToAdd + "</b> points de vie durant votre absence.");
                Client.Character.RefreshStats();
            }
        }

        public void StopRegenLife()
        {
            if (this.IsRegeneratingLife)
            {
                this.Client.Send(new LifePointsRegenEndMessage());
                var StartTime = DateTimeUtils.GetEpochFromDateTime(RegenStartTime);
                var CurrentTime = DateTimeUtils.GetEpochFromDateTime(DateTime.Now);
                var LifePointsToAdd = 0;
                while (StartTime < CurrentTime)
                {
                    if ((this.CurrentStats.LifePoints + LifePointsToAdd) >= this.CharacterStatsRecord.LifePoints)
                        break;
                    if (this.RegenRate == 10)
                        LifePointsToAdd++;
                    else if (this.RegenRate == 3)
                        LifePointsToAdd += 3;
                    StartTime++;
                }
                if (this.RegenRate == 3)
                    LifePointsToAdd++;
                this.CurrentStats.LifePoints += (uint)LifePointsToAdd;
                if (this.CurrentStats.LifePoints > this.CharacterStatsRecord.LifePoints)
                    this.CurrentStats.LifePoints = (uint)this.CharacterStatsRecord.LifePoints;
                this.Record.CurrentLifePoint = this.CurrentStats.LifePoints;
                this.RefreshStats();
                this.IsRegeneratingLife = false;
            }
        }

        public bool CanSendSalesMessage()
        {
            if (this.LastSalesMessage == 0)
                return true;
            else
            {
                var StartTime = LastSalesMessage;
                var Seconds = 0;
                var CurrentTime = DateTimeUtils.GetEpochFromDateTime(DateTime.Now);
                while (StartTime < CurrentTime)
                {
                    Seconds++;
                    StartTime++;
                }
                if (Seconds >= ConfigurationManager.Instance.TimeBetweenSalesMessage)
                    return true;
                else
                {
                    var Timeleft = ConfigurationManager.Instance.TimeBetweenSalesMessage - Seconds;
                    this.Reply("Ce canal est restreint pour améliorer sa lisibilité. Vous pourrez envoyer un nouveau message dans " + Timeleft + " secondes. Ceci ne vous autorise cependant pas pour autant à surcharger ce canal.");
                    return false;
                }
            }
        }

        public void UpdateLastSalesMessage()
        {
            this.LastSalesMessage = (int)DateTimeUtils.GetEpochFromDateTime(DateTime.Now);
        }

        public bool CanSendSeekMessage()
        {
            if (this.LastSalesMessage == 0)
                return true;
            else
            {
                var StartTime = LastSeekMessage;
                var Seconds = 0;
                var CurrentTime = DateTimeUtils.GetEpochFromDateTime(DateTime.Now);
                while (StartTime < CurrentTime)
                {
                    Seconds++;
                    StartTime++;
                }
                if (Seconds >= ConfigurationManager.Instance.TimeBetweenSeekMessage)
                    return true;
                else
                {
                    var Timeleft = ConfigurationManager.Instance.TimeBetweenSeekMessage - Seconds;
                    this.Reply("Ce canal est restreint pour améliorer sa lisibilité. Vous pourrez envoyer un nouveau message dans " + Timeleft + " secondes. Ceci ne vous autorise cependant pas pour autant à surcharger ce canal.");
                    return false;
                }
            }
        }

        public void UpdateLastSeekMessage()
        {
            this.LastSeekMessage = DateTimeUtils.GetEpochFromDateTime(DateTime.Now);
        }

        public bool CanSave()
        {
            if (this.LastCharacterSave == 0)
                return true;
            var StartTime = this.LastCharacterSave;
            var CurrentTime = DateTimeUtils.GetEpochFromDateTime(DateTime.Now);
            var seconds = 0;
            while (StartTime <= CurrentTime)
            {
                seconds++;
                StartTime++;
            }
            if (seconds < ConfigurationManager.Instance.TimeBetweenCharacterSave)
            {
                this.Reply("Vous devez attendre encore " + (ConfigurationManager.Instance.TimeBetweenCharacterSave - seconds) + " seconde(s) pour pouvoir à nouveau sauvegarder votre personnage.");
                return false;
            }
            else
                return true;
        }

        public bool CanStart()
        {
            if (this.LastCharacterStart == 0)
                return true;
            var StartTime = this.LastCharacterStart;
            var CurrentTime = DateTimeUtils.GetEpochFromDateTime(DateTime.Now);
            var seconds = 0;
            while (StartTime <= CurrentTime)
            {
                seconds++;
                StartTime++;
            }
            if (seconds < ConfigurationManager.Instance.TimeBetweenStart)
            {
                this.Reply("Vous devez attendre encore " + (ConfigurationManager.Instance.TimeBetweenStart - seconds) + " seconde(s) pour pouvoir à nouveau téléporter votre personnage.");
                return false;
            }
            else
                return true;
        }

        #region MerchantItems
        public void LoadMerchantItems()
        {
            this.MerchantItems = CharactersMerchantsRecord.GetCharactersItems(this.Id);
        }

        public uint GetTaxCost()
        {
            uint cost = 0;
            var quantityIndex = 0;
            uint tmp = 0;
            foreach (var item in this.MerchantItems)
            {
                quantityIndex = 0;
                while (quantityIndex <= item.Quantity)
                {
                    tmp = item.Price / 15000;
                    if (tmp != 0)
                        cost += tmp;
                    else
                        cost += 1;
                    quantityIndex++;
                }
            }
            return cost;
        }
        #endregion

        #region Friends, Enemies, Ignored

        #region Friends

        #region SendInformations

        private void SendWarnOnStateMessages()
        {
            this.Client.Send(new FriendWarnOnConnectionStateMessage(this.Account.WarnOnFriendConnection));
        }

        private void SendOnlineFriendsCountMessage()
        {
            int onlineFriendsCount = 0;
            this.Friends.ForEach((x) =>
            {
                var friendCharacters = CharacterRecord.GetAccountCharacters(x.FriendAccountId);
                foreach (var character in friendCharacters)
                {
                    var worldClient = WorldServer.Instance.GetOnlineClient(character.Id);
                    if (worldClient != null)
                    {
                        onlineFriendsCount = onlineFriendsCount + 1;
                    }
                }
            });
            if (onlineFriendsCount > 0)
            {
                this.Client.Send(new TextInformationMessage(0, 197, new string[1] { onlineFriendsCount.ToString() }));
            }
        }

        #endregion

        public void LoadFriends()
        {
            var accountFriends = FriendRecord.CharactersFriends.FindAll(x => x.AccountId == this.Record.AccountId);
            if(accountFriends != null && accountFriends.Count > 0)
            {
                this.Friends.AddRange(accountFriends);
                this.Friends.ForEach((x) =>
                {
                    var friendCharacters = CharacterRecord.GetAccountCharacters(x.FriendAccountId); 
                    if(friendCharacters.Count > 0)
                    {
                        friendCharacters.ForEach((friendCharacter) =>
                        {
                            var friendWorldClient = WorldServer.Instance.GetOnlineClient(friendCharacter.Name);
                            if(friendWorldClient != null && this.Account.WarnOnFriendConnection)
                            {
                                friendWorldClient.Send(new TextInformationMessage(0, 143, new string[] { this.Account.Nickname, this.Record.Name, this.Record.Id.ToString() }));
                            }
                        });
                    }
                });
            }
        }

        public FriendRecord AddFriend(int FriendAccountId)
        {
            var newFriend = new FriendRecord(FriendRecord.PopNextId(), this.Record.AccountId, FriendAccountId);
            this.Friends.Add(newFriend);
            return newFriend;
        }

        public bool RemoveFriend(int friendAccountId)
        {
            bool removed = false;
            var oldFriend = this.Friends.FirstOrDefault(x => x.FriendAccountId == friendAccountId);
            if (oldFriend != null)
            {
                this.Friends.Remove(oldFriend);
                SaveTask.RemoveElement(oldFriend, this.Id);
                removed = true;
            }
            return removed;
        }

        public bool IsFriendWith(int accountId)
        {
            bool isFriend = false;
            if(this.Friends.FirstOrDefault(x => x.FriendAccountId == accountId) != null)
            {
                isFriend = true;
            }
            return isFriend;
        }

        public string GetFriendName(int accountId)
        {
            string friendName = string.Empty;
            var friend = this.Friends.FirstOrDefault(x => x.FriendAccountId == accountId);
            if (friend != null)
            {
                var friendAccount = AccountsProvider.GetAccountFromDb(friend.AccountId);
                if (friendAccount != null)
                {
                    friendName = friendAccount.Nickname;
                }
            }
            return friendName;
        }

        #endregion

        #region Enemies

        public void LoadEnemies()
        {
            var accountEnemies = IgnoredRecord.CharactersIgnored.FindAll(x => x.AccountId == this.Record.AccountId);
            if (accountEnemies != null && accountEnemies.Count > 0)
            {
                this.Enemies.AddRange(accountEnemies);
            }
        }

        public IgnoredRecord AddEnemy(int enemyAccountId)
        {
            var newEnemy = new IgnoredRecord(IgnoredRecord.PopNextId(), this.Record.AccountId, enemyAccountId);
            this.Enemies.Add(newEnemy);
            return newEnemy;
        }

        public bool AlreadyEnemy(int accountId)
        {
            bool enemy = false;
            if(this.Enemies.FirstOrDefault(x => x.IgnoredAccountId == accountId) != null)
            {
                enemy = true;
            }
            return enemy;
        }

        public string GetEnemyName(int accountId)
        {
            string enemyName = string.Empty;
            var enemy = this.Enemies.FirstOrDefault(x => x.IgnoredAccountId == accountId);
            if(enemy != null)
            {
                var enemyAccount = AccountsProvider.GetAccountFromDb(enemy.AccountId);
                if(enemyAccount != null)
                {
                    enemyName = enemyAccount.Nickname;
                }
            }
            return enemyName;
        }

        public bool RemoveEnemy(int ignoredAccountId)
        {
            bool removed = false;
            var oldEnemy = this.Enemies.FirstOrDefault(x => x.IgnoredAccountId == ignoredAccountId);
            if (oldEnemy != null)
            {
                this.Enemies.Remove(oldEnemy);
                SaveTask.RemoveElement(oldEnemy, this.Id);
                removed = true;
            }
            return removed;
        }

        #endregion

        #region Ignored

        public bool AddIgnored(int ignoredAccountId)
        {
            bool added = false;
            this.Ignored.Add(new IgnoredRecord(1, this.Record.AccountId, ignoredAccountId));
            added = true;
            return added;
        }

        public bool AlreadyIgnored(int accountId)
        {
            bool enemy = false;
            if (this.Ignored.FirstOrDefault(x => x.IgnoredAccountId == accountId) != null)
            {
                enemy = true;
            }
            return enemy;
        }

        public string GetIgnoredName(int accountId)
        {
            string ignoredName = string.Empty;
            var ignored = this.Ignored.FirstOrDefault(x => x.IgnoredAccountId == accountId);
            if (ignored != null)
            {
                var ignoredAccount = AccountsProvider.GetAccountFromDb(ignored.AccountId);
                if (ignoredAccount != null)
                {
                    ignoredName = ignoredAccount.Nickname;
                }
            }
            return ignoredName;
        }

        public bool RemoveIgnored(int ignoredAccountId)
        {
            bool removed = false;
            var oldIgnored = this.Ignored.FirstOrDefault(x => x.IgnoredAccountId == ignoredAccountId);
            if (oldIgnored != null)
            {
                this.Ignored.Remove(oldIgnored);
                removed = true;
            }
            return removed;
        }

        #endregion

        public bool IsIgnoring(int accountId)
        {
            bool isIgnoring = false;
            if(this.Enemies.FirstOrDefault(x => x.IgnoredAccountId == accountId) != null || this.Ignored.FirstOrDefault(x => x.IgnoredAccountId == accountId) != null)
            {
                isIgnoring = true;
            }
            return isIgnoring;
        }

        #endregion

        #region Keyrings

        public bool HaveKeyring()
        {
            var playerItems = this.Inventory.GetAllItems();
            foreach (var item in playerItems)
                if (item.GID == 10207)
                    return true;
            return false;
        }

        public void DeleteKeyIfExist(int templateId)
        {
            foreach (var key in this.Keyrings)
            {
                if (key.KeyId == templateId)
                {
                    this.Keyrings.Remove(key);
                    break;
                }
            }
        }

        public bool CanUseKeyring(int keyTemplateId)
        {
            foreach (var key in this.Keyrings)
            {
                if (key.KeyId == keyTemplateId)
                {
                    var startTime = key.KeyTimeUse;
                    var currentTime = DateTimeUtils.GetEpochFromDateTime(DateTime.Now);
                    var seconds = 0;
                    while (startTime <= currentTime)
                    {
                        seconds++;
                        startTime++;
                    }
                    if (seconds >= ConfigurationManager.Instance.TimeForUseKeyring)
                        return true;
                    else
                    {
                        var timeLeft = ConfigurationManager.Instance.TimeForUseKeyring - seconds;
                        var time = timeLeft / 60 + " minute(s) restante(s)";
                        this.Reply("Vous ne possédez pas la clef pour rentrer dans ce donjon et il est encore trop tôt pour utiliser votre trousseau de clés (<b>" + time + "</b>).");
                        return false;
                    }
                }
            }
            return true;
        }

        public void UseKeyring(int keyTemplateId)
        {
            this.DeleteKeyIfExist(keyTemplateId);
            this.Keyrings.Add(new CharacterKeyring(keyTemplateId, DateTimeUtils.GetEpochFromDateTime(DateTime.Now)));
            this.Client.Character.Reply("Vous avez utilisé votre trousseau de clés pour entrer dans ce donjon car vous ne possédiez pas la clé.");
        }

        #endregion

        #region DelayedActions

        public void OnStartingUseDelayedObject(DelayedActionTypeEnum type, double delayEndTime, int itemId)
        {
            var clientsOnMap = WorldServer.Instance.GetOnlineClientOnMap(this.Record.MapId);
            foreach (var client in clientsOnMap)
            {
                if (client.Character.IsFighting == false && client.Character.Busy == false)
                    client.Send(new GameRolePlayDelayedObjectUseMessage(this.Id, (sbyte)type, delayEndTime, (ushort)itemId));
            }
        }

        public void OnEndingUseDelayedObject(DelayedActionTypeEnum type)
        {
            var clientsOnMap = WorldServer.Instance.GetOnlineClientOnMap(this.Record.MapId);
            foreach (var client in clientsOnMap)
            {
                if (client.Character.IsFighting == false && client.Character.Busy == false)
                    client.Send(new GameRolePlayDelayedActionFinishedMessage(this.Id, (sbyte)type));
            }
        }

        #endregion

        #region Tracking

        public void OnTrackingTimeElapsed(WorldClient target, Timer actionTimer, CharacterItemRecord itemRecord)
        {
            this.OnEndingUseDelayedObject(DelayedActionTypeEnum.DELAYED_ACTION_OBJECT_USE);
            if (target.Character.CurrentlyInTrackRequest)
            {
                target.Character.Reply("Le joueur <b>" + this.Record.Name + "</b>" + " détient désormais un parchemin de recherche lié à votre nom !", Color.Orange);
                this.Inventory.RemoveItem(itemRecord.UID, 1);

                List<ObjectEffect> objectEffects = new List<ObjectEffect>();
                objectEffects.Add(new ObjectEffectString(989, target.Character.Record.Name));
                CharacterItemRecord newItem = new CharacterItemRecord(CharacterItemRecord.PopNextUID(), 63, 7400, this.Id, 1, objectEffects);
                this.Inventory.Add(newItem);
                this.Reply("Vous avez obtenu 1 <b>Parchemin lié</b> !");

                TrackRecord.AddTracked(target.Character.Record.Id, (int)newItem.UID);
                target.Character.CurrentlyInTrackRequest = false;
            }
            else
            {
                this.Inventory.RemoveItem(itemRecord.UID, 1);
                this.Reply("Vous n'avez pas obtenu de parchemin lié à <b>" + target.Character.Record.Name + "</b> car il s'est enfuit !", Color.Orange);
                target.Character.Reply("Un joueur a essayé de vous traquer mais vous avez fui !", Color.Orange);
            }

            this.IsTracking = false;
            actionTimer.Stop();
        }

        #endregion

        public void increasePlayersKilled(int numbers)
        {
            this.Record.PlayersKilled = this.Record.PlayersKilled + numbers;
        }

        public void resetPlayersKilled()
        {
            this.Record.PlayersKilled = 0;
        }

        public CharacterItemRecord haveGuildalogemme()
        {
            var items = this.Inventory.GetAllItems();

            foreach (var item in items)
            {
                if (item.GID == 1575) // ID guildalogemme
                    return (item);
            }
            return null;
        }
    }
}
