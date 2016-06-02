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
using Symbioz.World.Models.Parties.Dungeon;
using Symbioz.World.Records;
using Symbioz.World.Records.Alliances;
using Symbioz.World.Records.Companions;
using Symbioz.World.Records.Guilds;
using Symbioz.World.Records.Maps;
using Symbioz.World.Records.Monsters;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Symbioz.World.Models
{
    public class Character : IDisposable
    {
        public Dictionary<Type, List<ITable>> _newElements = new Dictionary<Type, List<ITable>>();
        public Dictionary<Type, List<ITable>> _updateElements = new Dictionary<Type, List<ITable>>();
        public Dictionary<Type, List<ITable>> _removeElements = new Dictionary<Type, List<ITable>>();
        public bool isGod = false;
        public bool isDebugging = false;
        public int Id { get { return Record.Id; } }
        public bool IsNew = false;
        public ushort SubAreaId { get; set; }
        public MapRecord Map { get; set; }
        public short MovedCell { get; set; }
        public WorldClient Client { get; set; }
        public ContextActorLook Look { get; set; }
        public CharacterRecord Record { get; set; }
        public bool Busy { get { return ExchangeType != null; } }
        public Inventory Inventory { get; set; }
        public StatsRecord StatsRecord { get { return StatsRecord.Stats.Find(x => x.CharacterId == Id); } }
        public BasicStats CurrentStats { get; set; }
        public List<HumanOption> HumanOptions { get; set; }
        public ActorRestrictionsInformations Restrictions = CharacterHelper.GetDefaultRestrictions();
        public DialogTypeEnum? CurrentDialogType = null;
        public List<SpellItem> Spells { get { return CharacterSpellRecord.GetCharacterSpells(Id); } }
        public List<ShortcutSpell> SpellsShortcuts { get { return SpellShortcutRecord.GetCharacterShortcuts(Id); } }
        public List<Shortcut> GeneralShortcuts { get { return GeneralShortcutRecord.GetCharacterShortcuts(Id); } }

        public PartyMember PartyMember;

        #region Exchanges
        public ExchangeTypeEnum? ExchangeType = null;
        public NpcBuySellExchange NpcShopExchange { get; set; }
        public SmithMagicExchange SmithMagicInstance { get; set; }
        public BankExchange BankInstance { get; set; }
        public BidShopExchange BidShopInstance { get; set; }
        public PlayerTradeExchange PlayerTradeInstance { get; set; }
        public CraftExchange CraftInstance { get; set; }
        public GuildInvitationDialog GuildInvitationDialog { get; set; }
        #endregion


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
                return (int)(StatsRecord.Strength + StatsRecord.Intelligence + StatsRecord.Agility +
                    StatsRecord.Chance + StatsRecord.Initiative * (CurrentStats.LifePoints / StatsRecord.LifePoints));
            }
        }

        public void AddElement(ITable element, bool addtolist = true)
        {
            lock (_newElements)
            {
                if (_newElements.ContainsKey(element.GetType()))
                {
                    if (!_newElements[element.GetType()].Contains(element))
                        _newElements[element.GetType()].Add(element);
                }
                else
                {
                    _newElements.Add(element.GetType(), new List<ITable> { element });
                }
            }
            if (addtolist)
            {
                #region Add value into array
                var field = GetCache(element);
                if (field == null)
                {
                    Logger.Error("Unable to add record value to the list, static list field wasnt finded");
                    return;
                }

                var method = field.FieldType.GetMethod("Add");
                if (method == null)
                {
                    Console.WriteLine("Unable to add record value to the list, add method wasnt finded");
                    return;
                }

                method.Invoke(field.GetValue(null), new object[] { element });
                #endregion
            }
        }

        public void UpdateElement(ITable element)
        {
            lock (_updateElements)
            {
                if (_newElements.ContainsKey(element.GetType()) && _newElements[element.GetType()].Contains(element))
                    return;

                if (_updateElements.ContainsKey(element.GetType()))
                {
                    if (!_updateElements[element.GetType()].Contains(element))
                        _updateElements[element.GetType()].Add(element);
                }
                else
                {
                    _updateElements.Add(element.GetType(), new List<ITable> { element });
                }
            }
        }

        public void RemoveElement(ITable element, bool removefromlist = true)
        {
            if (element == null)
                return;
            lock (_removeElements)
            {
                if (_newElements.ContainsKey(element.GetType()) && _newElements[element.GetType()].Contains(element))
                {
                    RemoveFromList(element);
                    _newElements[element.GetType()].Remove(element);
                    return;
                }

                if (_updateElements.ContainsKey(element.GetType()) && _updateElements[element.GetType()].Contains(element))
                    _updateElements[element.GetType()].Remove(element);

                if (_removeElements.ContainsKey(element.GetType()))
                {
                    if (!_removeElements[element.GetType()].Contains(element))
                        _removeElements[element.GetType()].Add(element);
                }
                else
                {
                    _removeElements.Add(element.GetType(), new List<ITable> { element });
                }
            }
            if (removefromlist)
            {
                RemoveFromList(element);
            }
        }

        private static FieldInfo GetCache(ITable table)
        {
            var attribute = table.GetType().GetCustomAttribute(typeof(TableAttribute), false);
            if (attribute == null)
                return null;

            var field = table.GetType().GetFields().FirstOrDefault(x => x.Name.ToLower() == (attribute as TableAttribute).tableName.ToLower());
            if (field == null || !field.IsStatic || !field.FieldType.IsGenericType)
                return null;

            return field;
        }

        static void RemoveFromList(ITable element)
        {
            var field = GetCache(element);
            if (field == null)
            {
                Console.WriteLine("[Remove] Erreur ! Field unknown");
                return;
            }

            var method = field.FieldType.GetMethod("Remove");
            if (method == null)
            {
                Console.WriteLine("[Remove] Erreur ! Field unknown");
                return;
            }

            method.Invoke(field.GetValue(null), new object[] { element });
        }

        public void ClearSave()
        {
            try
            {
                var types = _removeElements.Keys.ToList();
                foreach (var type in types)
                {
                    List<ITable> elements;
                    lock (_removeElements)
                        elements = _removeElements[type];
                    lock (_removeElements)
                        _removeElements[type] = _removeElements[type].Skip(elements.Count).ToList();
                }

                types = _newElements.Keys.ToList();
                foreach (var type in types)
                {
                    List<ITable> elements;

                    lock (_newElements)
                        elements = _newElements[type];
                    lock (_newElements)
                        _newElements[type] = _newElements[type].Skip(elements.Count).ToList();
                }

                types = _updateElements.Keys.ToList();
                foreach (var type in types)
                {
                    List<ITable> elements;
                    lock (_updateElements)
                        elements = _updateElements[type];
                    lock (_updateElements)
                    {
                        var attribute = (TableAttribute)type.GetCustomAttribute(typeof(TableAttribute));

                        if (attribute != null && !attribute.letInUpdateField)
                            _updateElements[type] = _updateElements[type].Skip(elements.Count).ToList();
                    }
                }
            }
            catch (Exception e) { Logger.Error("[CLEAR SAVE CHARACTER " + this.Record.Name + "]" + e.Message); }
        }

        public void Save()
        {
            try
            {
                var types = _removeElements.Keys.ToList();
                foreach (var type in types)
                {
                    List<ITable> elements;
                    lock (_removeElements)
                        elements = _removeElements[type];

                    try
                    {
                        var writer = Activator.CreateInstance(typeof(DatabaseWriter<>).MakeGenericType(type), DatabaseAction.Remove, elements.ToArray());
                    }
                    catch (Exception e) { Logger.Error(e.Message); }

                    lock (_removeElements)
                        _removeElements[type] = _removeElements[type].Skip(elements.Count).ToList();

                }

                types = _newElements.Keys.ToList();
                foreach (var type in types)
                {
                    List<ITable> elements;

                    lock (_newElements)
                        elements = _newElements[type];

                    try
                    {
                        var writer = Activator.CreateInstance(typeof(DatabaseWriter<>).MakeGenericType(type), DatabaseAction.Add, elements.ToArray());
                    }
                    catch (Exception e) { Logger.Error(e.ToString()); }

                    lock (_newElements)
                        _newElements[type] = _newElements[type].Skip(elements.Count).ToList();

                }

                types = _updateElements.Keys.ToList();
                foreach (var type in types)
                {
                    List<ITable> elements;
                    lock (_updateElements)
                        elements = _updateElements[type];
                    try
                    {
                        var writer = Activator.CreateInstance(typeof(DatabaseWriter<>).MakeGenericType(type), DatabaseAction.Update, elements.ToArray());
                    }
                    catch (Exception e) { Logger.Error(e.ToString()); }

                    lock (_updateElements)
                    {
                        var attribute = (TableAttribute)type.GetCustomAttribute(typeof(TableAttribute));

                        if (attribute != null && !attribute.letInUpdateField)
                            _updateElements[type] = _updateElements[type].Skip(elements.Count).ToList();
                    }
                }
                this.Reply("Votre personnage a été sauvegardé avec succès.");
                Console.WriteLine(")) Character " + this.Record.Name + " was saved !");
            }
            catch (Exception e) { Logger.Error("[SAVING CHARACTER " + this.Record.Name + "]" + e.Message); }
        }

        public PlayerStatus PlayerStatus;

        public Character(CharacterRecord record, WorldClient client)
        {
            this.Client = client;
            this.Record = record;
            this.Look = ContextActorLook.Parse(Record.Look);
            this.HumanOptions = new List<HumanOption>();

            this.Inventory = new Inventory(this);

            this.PlayerStatus = new PlayerStatus((sbyte)PlayerStatusEnum.PLAYER_STATUS_AVAILABLE);
        }
        public void OnConnectedGuildInformations()
        {
            if (HasGuild)
            {
                Client.Send(new GuildMembershipMessage(this.GetGuild().GetGuildInformations(), CharacterGuildRecord.GetCharacterGuild(this.Id).Rights, true));
                this.HumanOptions.Add(new HumanOptionGuild(this.GetGuild().GetGuildInformations()));
                Client.Character.RefreshOnMapInstance();
            }
        }
        public void OnConnectedAllianceInformations()
        {
            if (HasAlliance)
            {
                Client.Send(new AllianceMembershipMessage(GetAlliance().GetAllianceInformations(), true));
                this.HumanOptions.Add(new HumanOptionAlliance(GetAlliance().GetAllianceInformations(),(sbyte)0));
                Client.Character.RefreshOnMapInstance();
            }
        }
        public CharacterFighter CreateFighter(FightTeam team)
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
                var cfighter = new CompanionFighter(EquipedCompanion, fighter, team);
                team.AddFighter(cfighter);
            }
            return fighter;
        }
        public void SendMap(Message message)
        {
            if (Map != null && Map.Instance != null)
                Map.Instance.Send(message);
        }
        public void OnConnectedNotifications()
        {
            Client.Send(new TextInformationMessage((sbyte)TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 89, new string[0]));
            Client.Character.Reply(ConfigurationManager.Instance.WelcomeMessage, System.Drawing.Color.GhostWhite);
            BidShopsHandler.AddEventualBidShopGains(Client);
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
            AggressableStatusEnum agressableStatus = Record.PvPEnable ? AggressableStatusEnum.PvP_ENABLED_AGGRESSABLE : AggressableStatusEnum.NON_AGGRESSABLE;
            var align = new ActorExtendedAlignmentInformations(Record.AlignmentSide, (sbyte)(Record.PvPEnable ? Record.AlignmentValue : 0), (sbyte)(Record.PvPEnable ? Record.AlignmentGrade : 0), Record.CharacterPower,
               Record.Honor, ExperienceRecord.GetHonorForGrade(Record.AlignmentGrade), ExperienceRecord.GetHonorForGrade((sbyte)(Record.AlignmentGrade + 1)), (sbyte)agressableStatus);
            return align;
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
                SaveTask.AddElement(spellRecord);
                SaveTask.AddElement(shortcutRecord);
                this.AddElement(spellRecord);
                this.AddElement(shortcutRecord);
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
            foreach(EmoteRecord emote in EmoteRecord.Emotes)
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
            if (this.PartyMember != null)
            {
                foreach (WorldClient c in this.PartyMember.Party.Members)
                {
                    c.Send(new PartyUpdateMessage((uint)this.PartyMember.Party.Id,
                        this.PartyMember.GetPartyMemberInformations()));
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
                if (Record.Level == 100)
                {
                    StatsRecord.ActionPoints++;
                    LearnEmote(22, sendpackets);
                }
                StatsRecord.LifePoints += 5;
                CurrentStats.LifePoints += 5;
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
            this.UpdateElement(job);
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
            Logger.Log(id);
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
            if (winner && FighterInstance.Fight.Map.DugeonMap)
            {
                var dungeonMap = DungeonRecord.GetDungeonData(FighterInstance.Fight.Map.Id);
                FighterInstance = null;
                Client.Character.Teleport(dungeonMap.TeleportMapId, dungeonMap.TeleportCellId);
                RefreshStats();
                return;
            }
            FighterInstance = null;
            if (spawnjoin && !winner)
            {
                if (Client.Character.Record.SpawnPointMapId != -1)
                {
                    MapsHandler.SendCurrentMapMessage(Client, Client.Character.Record.SpawnPointMapId);
                    Record.CellId = (short)InteractiveRecord.GetTeleporterCellId(Client.Character.Record.SpawnPointMapId, TeleporterTypeEnum.TELEPORTER_ZAAP);
                }
                else
                    Client.Character.Teleport(ConfigurationManager.Instance.StartMapId, ConfigurationManager.Instance.StartCellId);
            }
            else
                MapsHandler.SendCurrentMapMessage(Client, Client.Character.Record.MapId);
            RefreshStats();

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
                    Reply("Vous avez perdu " + amount + " kama(s)");
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
            Client.Send(new CharacterStatsListMessage(StatsRecord.GetCharacterCharacteristics(StatsRecord, this)));
            this.RefreshGroupInformations();
        }
        public void LeaveDialog()
        {
            ContextRolePlayHandler.HandleLeaveDialog(null, Client);
        }
        public void LeaveExchange()
        {
            switch (ExchangeType)
            {
                case ExchangeTypeEnum.PLAYER_TRADE:
                    if (PlayerTradeInstance != null)
                        PlayerTradeInstance.CancelExchange();
                    break;
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
                    break;
                case ExchangeTypeEnum.RUNES_TRADE:
                    if (SmithMagicInstance != null)
                        SmithMagicInstance.CancelExchange();
                    break;
                case ExchangeTypeEnum.NPC_SHOP:
                    if (NpcShopExchange != null)
                        NpcShopExchange.CancelExchange();
                    break;
            }

            Client.Send(new ExchangeLeaveMessage((sbyte)DialogTypeEnum.DIALOG_EXCHANGE, true));
            ExchangeType = null;
        }

        public void Dispose()
        {
            if (Map != null)
                Map.Instance.RemoveClient(Client);
            if (SearchingArena)
                ArenaProvider.Instance.OnClientDisconnected(Client);
            if (IsFighting)
                FighterInstance.OnDisconnect();
            if (PlayerTradeInstance != null)
                PlayerTradeInstance.Abort();
            if (PartyMember != null)
                PartyMember.Party.QuitParty(Client);
            if (DungeonPartyProvider.Instance.GetDPCByCharacterId(this.Id) != null)
                DungeonPartyProvider.Instance.RemoveCharacter(this);
            Client.Character.Look.UnsetAura();
            Record.Look = Look.ConvertToString();
            SaveTask.UpdateElement(Record);
            SaveTask.UpdateElement(StatsRecord);
            this.UpdateElement(Record);
            this.UpdateElement(StatsRecord);
            Inventory.InitializeForSaveTask();
        }

        public void RefreshGroupInformations()
        {
            if (this.PartyMember != null)
            {
                foreach (WorldClient client in this.PartyMember.Party.Members)
                {
                    client.Send(new PartyUpdateMessage((uint)this.PartyMember.Party.Id,
                        this.PartyMember.GetPartyMemberInformations()));
                }
            }
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

        public bool HasAlliance {
            get
            {
                if (HasGuild)
                {
                    if (GuildAllianceRecord.GuildsAlliances.Find(x => x.GuildId == GetGuild().Id) != null){
                        return true;
                    }
                }
                return false;
            }
        }
    }
}
