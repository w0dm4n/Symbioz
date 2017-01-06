using Symbioz.World.Models.Fights.Fighters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Symbioz.DofusProtocol.Types;
using Symbioz.World.Models.Fights;
using Symbioz.World.Records.Monsters;
using Symbioz.Helper;
using Symbioz.Core;
using Symbioz.Network.Clients;
using Symbioz.World.Models.Monsters;
using Symbioz.World.Records;
using Symbioz.Providers.FightResults.Exp;
using Symbioz.World.Models.Fights.FightsTypes;
using Symbioz.World.Models;
using Symbioz.Network.Servers;
using Symbioz.Network;
using Shader.Helper;
using Symbioz.DofusProtocol.Messages;
using System.Drawing;
using Symbioz.World.Records.Items;
using Symbioz.World.Records.Alliances.Prisms;

namespace Symbioz.Providers.FightResults
{
    /// <summary>
    /// a clean , et gestion XPGuilde
    /// </summary>
    public class FightResultPlayer : FightResult
    {
        public List<FightResultAdditionalData> AdditionalDatas = new List<FightResultAdditionalData>();
        public byte PlayerLevel { get; set; }
        public FightLoot FightLoot = new FightLoot(new List<ushort>(), 0);
        public FightResultPlayer(CharacterFighter fighter, TeamColorEnum winner, ulong xpwinnable, bool forLeaver = false)
            : base(fighter, winner)
        {
            this.PlayerLevel = fighter.Client.Character.Record.Level;
            if (!forLeaver)
            {
                WorldClient client = (Fighter as CharacterFighter).Client;
                if (winner == fighter.Team.TeamColor)
                {
                    if (ConfigurationManager.Instance.ServerId == 22 && fighter.Fight is FightAgression)
                    {
                        if (!fighter.Fight.DeadItemsLoaded)
                            fighter.Fight.LoadDeadItems((fighter.Team.TeamColor == TeamColorEnum.BLUE_TEAM) ? fighter.Fight.RedTeam.GetCharacterFighters(true) : fighter.Fight.BlueTeam.GetCharacterFighters(true));
                        GeneratePlayerLoot((fighter.Team.TeamColor == TeamColorEnum.BLUE_TEAM) ? fighter.Fight.BlueTeam.GetCharacterFighters(true) : fighter.Fight.RedTeam.GetCharacterFighters(true), fighter);
                        fighter.AlreadyDropped = true;

                        //gain de 25% des gagnants
                        var expWon = xpwinnable;
                        client.Character.AddXp((ulong)expWon);
                        PlayerLevel = client.Character.Record.Level;
                        var expdatas = new FightResultExperienceData(true, true, true, true, false, false, false, client.Character.Record.Exp, ExperienceRecord.GetExperienceForLevel(client.Character.Record.Level), ExperienceRecord.GetExperienceForLevel((uint)client.Character.Record.Level + 1), (int)expWon, 0, 0, 0);
                        AdditionalDatas.Add(expdatas);
                    }
                    else if (fighter.Fight is FightPvM)
                        GeneratePVMLoot();
                }
                else if (fighter.Fight is FightPvM && winner != fighter.Team.TeamColor)
                {
                    if (ConfigurationManager.Instance.ServerId == 22)
                    {
                        if (!client.Character.Restrictions.isDead)
                        {
                            client.Character.Record.DeathCount++;
                            if (client.Character.Record.DeathMaxLevel < client.Character.Record.Level)
                                client.Character.Record.DeathMaxLevel = client.Character.Record.Level;
                            client.Character.Record.Energy = 0;
                            client.Character.Look = ContextActorLook.Parse("{24}");
                            client.Character.Restrictions.cantMove = true;
                            client.Character.Restrictions.cantSpeakToNPC = true;
                            client.Character.Restrictions.cantExchange = true;
                            client.Character.Restrictions.cantAttackMonster = true;
                            client.Character.Restrictions.isDead = true;
                            String[] Data = new string[1];
                            Data[0] = client.Character.Record.Name;
                            client.Send(new TextInformationMessage(1, 190, Data));
                        }
                    }
                    var groupSubArea = MonsterSpawnManager.GetMonsterSpawnBySubArea(fighter.Fight.Map.SubAreaId);
                    if(groupSubArea != null)
                    {
                        for (int i = 0; i < (fighter as CharacterFighter).Client.Character.Inventory.GetAllItems().Count; i++)
                        {
                            Random rdm = new Random();
                            var group = groupSubArea.Find(x => x.MonsterGroupId == rdm.Next(MonsterGroup.START_ID, groupSubArea.Last().MonsterGroupId));
                            while (group == null)
                            {
                                group = groupSubArea.Find(x => x.MonsterGroupId == rdm.Next(MonsterGroup.START_ID, groupSubArea.Last().MonsterGroupId));
                            }
                            if (group != null)
                            {
                                var item = (fighter as CharacterFighter).Client.Character.Inventory.GetAllItems()[i];
                                group.ItemsToDrop.Add(item);
                            }
                        }  
                    }
                }
                if (fighter.Fight is FightAvAPrism)
                {
                    if (winner != fighter.Team.TeamColor && ConfigurationManager.Instance.ServerId == 22)
                    {
                        client.Character.Record.DeathCount++;
                        if (client.Character.Record.DeathMaxLevel < client.Character.Record.Level)
                            client.Character.Record.DeathMaxLevel = client.Character.Record.Level;
                        client.Character.Record.Energy = 0;
                        client.Character.Look = ContextActorLook.Parse("{24}");
                        client.Character.Restrictions.cantMove = true;
                        client.Character.Restrictions.cantSpeakToNPC = true;
                        client.Character.Restrictions.cantExchange = true;
                        client.Character.Restrictions.cantAttackMonster = true;
                        client.Character.Restrictions.isDead = true;
                        String[] Data = new string[1];
                        Data[0] = client.Character.Record.Name;
                        client.Send(new TextInformationMessage(1, 190, Data));
                    }
                    else if (winner == fighter.Team.TeamColor && ConfigurationManager.Instance.ServerId == 22)
                    {
                        if (!fighter.Fight.DeadItemsLoaded)
                            fighter.Fight.LoadDeadItems((fighter.Team.TeamColor == TeamColorEnum.BLUE_TEAM) ? fighter.Fight.RedTeam.GetCharacterFighters(true) : fighter.Fight.BlueTeam.GetCharacterFighters(true));
                        GeneratePlayerLoot((fighter.Team.TeamColor == TeamColorEnum.BLUE_TEAM) ? fighter.Fight.BlueTeam.GetCharacterFighters(true) : fighter.Fight.RedTeam.GetCharacterFighters(true), fighter);
                        fighter.AlreadyDropped = true;
                    }
                }
                if (fighter.Fight is FightArena && winner == fighter.Team.TeamColor)
                {
                    GenerateArenaLoot();
                }

                if (fighter.Fight is FightAgression && winner != fighter.Team.TeamColor && ConfigurationManager.Instance.ServerId == 22)
                {
                    if (!client.Character.Restrictions.isDead)
                    {
                        client.Character.Record.DeathCount++;
                        if (client.Character.Record.DeathMaxLevel < client.Character.Record.Level)
                            client.Character.Record.DeathMaxLevel = client.Character.Record.Level;
                        client.Character.Record.Energy = 0;
                        client.Character.Look = ContextActorLook.Parse("{24}");
                        client.Character.Restrictions.cantMove = true;
                        client.Character.Restrictions.cantSpeakToNPC = true;
                        client.Character.Restrictions.cantExchange = true;
                        client.Character.Restrictions.cantAttackMonster = true;
                        client.Character.Restrictions.isDead = true;
                        String[] Data = new string[1];
                        Data[0] = client.Character.Record.Name;
                        client.Send(new TextInformationMessage(1, 190, Data));
                    }
                }
                if (fighter.Disconnected)
                {
                    CharactersDisconnected.remove(client.Character.Record.Name);
                    WorldServer.Instance.WorldClients.Remove(client);
                }
            }
        }

        public override FightResultListEntry GetEntry()
        {
            return new FightResultPlayerListEntry((ushort)OutCome, 0, FightLoot, Fighter.ContextualId, !Fighter.Dead, PlayerLevel, AdditionalDatas);
        }

        public FightResultListEntry GetEntryForLeaver()
        {
            return new FightResultPlayerListEntry((ushort)OutCome, 0, FightLoot, Fighter.ContextualId, !Fighter.Dead, PlayerLevel, AdditionalDatas);
        }

        public void GenerateArenaLoot()
        {
            if ((Fighter as CharacterFighter).HasLeft)
                return;

            WorldClient client = (Fighter as CharacterFighter).Client;
            AsyncRandom random = new AsyncRandom();

            FightLoot.kamas += (uint)(random.Next(50 * client.Character.Record.Level, 250 * client.Character.Record.Level));
            client.Character.AddKamas((int)FightLoot.kamas);

            uint itemQt = (uint)random.Next(1, client.Character.Record.Level);
            FightLoot.objects.Add(FightArena.ARENA_ITEM_ID);
            FightLoot.objects.Add((ushort)itemQt);
            client.Character.Inventory.Add(FightArena.ARENA_ITEM_ID, itemQt);

            if (client.Character.Record.Level != 200)
            {
                var experienceForNextLevel = ExperienceRecord.GetExperienceForLevel((uint)client.Character.Record.Level + 1);
                var experienceForLevel = ExperienceRecord.GetExperienceForLevel(client.Character.Record.Level);
                int earnedXp = (int)((double)(experienceForNextLevel - (double)experienceForLevel) / (double)15);

                var expdatas = new FightResultExperienceData(true, true, true, true, false, false, false, client.Character.Record.Exp, experienceForLevel, experienceForNextLevel, earnedXp, 0, 0, 0);
                AdditionalDatas.Add(expdatas);
                client.Character.AddXp((ulong)earnedXp);
            }
        }

        public void GeneratePlayerLoot(List<CharacterFighter> Winners, CharacterFighter fighter)
        {
            WorldClient client = (Fighter as CharacterFighter).Client;
            var WinnerNumber = Winners.Count;
            int killedPlayers = 0;
            var willDrop = fighter.Fight.ListDeadsItemsStartSize / Winners.Count;
            int[] dropped = new int[willDrop];
            int index = 0;
            int infinite = 0;
            Random rand = new Random();

            if (fighter.Team.TeamColor == TeamColorEnum.BLUE_TEAM)
                killedPlayers = fighter.Fight.RedTeam.GetCharacterFighters(true).Count;
            else if (fighter.Team.TeamColor == TeamColorEnum.RED_TEAM)
                killedPlayers = fighter.Fight.BlueTeam.GetCharacterFighters(true).Count;
            client.Character.increasePlayersKilled(killedPlayers);
            while (willDrop != 0)
            {
                if (Winners.Count != 1)
                {
                    infinite = 0;
                    while (true)
                    {
                        var randomValue = rand.Next(0, fighter.Fight.ListDeadItems.Count);
                        if (!ArrayUtils.InArray(dropped, randomValue))
                        {
                            dropped[index] = randomValue;
                            break;
                        }
                        infinite++;
                        if (infinite > 100000)
                            break;
                    }
                }
                else
                    dropped[index] = index;
                index++;
                willDrop--;
            }
            var ListDropped = fighter.Fight.GetItemRecordDropped(dropped);
            foreach (var item in ListDropped)
            {
                if (item != null)
                {
                    ItemRecord template = ItemRecord.GetItem(item.Id);
                    if (template != null)
                    {
                        FightLoot.objects.Add((ushort)item.Id);
                        FightLoot.objects.Add(1);
                        if (fighter.AlreadyDropped == false)
                        {
                            client.Character.Inventory.Add((ushort)item.Id, 1, false, false);
                            fighter.Fight.DeleteFromDeadItems(item.Id);
                        }
                    }
                    else
                    {
                        FightLoot.objects.Add((ushort)item.Id);
                        FightLoot.objects.Add(1);
                        if (fighter.AlreadyDropped == false)
                        {
                            client.Character.Inventory.AddWeapon((ushort)item.Id, 1, false, false);
                            fighter.Fight.DeleteFromDeadItems(item.Id);
                        }
                    }
                }
             }
            client.Character.Inventory.Refresh();
            client.Character.RefreshShortcuts();
        }


        static void SendNotif(System.Timers.Timer Timer, WorldClient client)
        {
            client.Character.ShowNotification("Vous avez obtenu un bonus d'expérience (<b>X2</b>) et de drop (<b>X2</b>) car votre alliance possède ce territoire !");
            Timer.Enabled = false;
            Timer.Stop();
            Timer.Dispose();
        }
        public double CheckDropBonusPrism(double dropChance, Fight fight, WorldClient client)
        {
            bool OwnTerritory = false;
            if (PrismRecord.PrismOnSubArea(fight.Map.SubAreaId))
            {
                if (client.Character.HasAlliance)
                {
                    var alliance = client.Character.GetAlliance();
                    if (alliance != null)
                    {
                        var prisms = PrismRecord.GetAlliancePrisms(alliance.Id);
                        foreach (var prism in prisms)
                        {
                            if (prism.SubAreaId == fight.Map.SubAreaId)
                            {
                                OwnTerritory = true;
                                break;
                            }
                        }
                        if (OwnTerritory == true)
                        {
                            var Timer = new System.Timers.Timer();
                            Timer.Interval = 1500;
                            Timer.Elapsed += (sender, e) => { SendNotif(Timer, client); };
                            Timer.Enabled = true;
                            return dropChance * 1.5;
                        }
                    }
                }
            }
            return dropChance;
        }

        public double CheckExpBonusPrism(double exp, Fight fight, WorldClient client)
        {
            bool OwnTerritory = false;
            if (PrismRecord.PrismOnSubArea(fight.Map.SubAreaId))
            {
                if (client.Character.HasAlliance)
                {
                    var alliance = client.Character.GetAlliance();
                    if (alliance != null)
                    {
                        var prisms = PrismRecord.GetAlliancePrisms(alliance.Id);
                        foreach (var prism in prisms)
                        {
                            if (prism.SubAreaId == fight.Map.SubAreaId)
                            {
                                OwnTerritory = true;
                                break;
                            }
                        }
                        if (OwnTerritory == true)
                        {
                            return exp * 1.5;
                        }
                    }
                }
            }
            return exp;
        }

        public void GeneratePVMLoot()
        {
            if ((Fighter as CharacterFighter).HasLeft)
                return;

            #region VariableDefinitions
            WorldClient client = (Fighter as CharacterFighter).Client;
            AsyncRandom random = new AsyncRandom();
            FightPvM pvmfight = Fighter.Fight as FightPvM;
            #endregion

            #region Kamas & Items Generation
            List<DroppedItem> m_drops = new List<DroppedItem>();
            List<CharacterItemRecord> m_remove = new List<CharacterItemRecord>();
            foreach(var items in pvmfight.MonsterGroup.ItemsToDrop)
            {
                if(items != null)
                {
                    items.CharacterId = client.CharacterId;
                    client.Character.Inventory.AddItemRecordWithQuantity(items, items.Quantity, true);
                    m_remove.Add(items);
                }
            }
            for (int i = 0; i < m_remove.Count; i++)
            {
                pvmfight.MonsterGroup.ItemsToDrop.Remove(m_remove[i]);
            }
            foreach (var monster in pvmfight.MonsterGroup.Monsters)
            {
                var template = MonsterRecord.GetMonster(monster.MonsterId);
                var grade = template.GetGrade(monster.ActualGrade);

                #region kamas
                int droppedKamas = random.Next(template.MinKamas, template.MaxKamas + 1);
                FightLoot.kamas += (uint)(droppedKamas * ConfigurationManager.Instance.KamasDropRatio);
                #endregion

                #region items
                List<CharacterFighter> charactersFighters = Fighter.Team.GetFighters().FindAll(x => x is CharacterFighter).ConvertAll<CharacterFighter>(x => (CharacterFighter)x);
                int prospectingSum = charactersFighters.Sum((CharacterFighter entry) => entry.Client.Character.CharacterStatsRecord.Prospecting);

                foreach (var item in template.Drops.FindAll(x => x.ProspectingLock <= prospectingSum))
                {
                    int D = random.Next(0, 201);
                    double dropchancePercent = (((item.GetDropRate(monster.ActualGrade) + pvmfight.MonsterGroup.AgeBonus / 5 + client.Character.CharacterStatsRecord.Prospecting / 100) / 4.5) * ConfigurationManager.Instance.ItemsDropRatio);
                    if (pvmfight.ChallengesInstance != null)
                        dropchancePercent = pvmfight.ChallengesInstance.GetDropBonus(dropchancePercent);
                    dropchancePercent = this.CheckDropBonusPrism(dropchancePercent, this.Fighter.Fight, client);
                    if ((int)dropchancePercent == 0)
                        dropchancePercent = 2;
                    if (D <= dropchancePercent)
                    {
                        var alreadyDropped = m_drops.FirstOrDefault(x => x.GID == item.ObjectId);
                        if (alreadyDropped == null)
                        {
                            uint dropMax = GetQuantityDropMax();

                            uint Q = (uint)random.Next(1, (int)(dropMax + 1));
                            if (Q > item.Count)
                                Q = 1;
                            ItemRecord templateItem = ItemRecord.GetItem(item.ObjectId);
                            if (templateItem != null)
                            {
                                m_drops.Add(new DroppedItem(item.ObjectId, Q));
                            }
                            else
                            {
                                WeaponRecord templateWeapon = WeaponRecord.GetWeapon(item.ObjectId);
                                if (templateWeapon != null)
                                {
                                    if (client.Character.Record.Level >= templateWeapon.Level)
                                        m_drops.Add(new DroppedItem(item.ObjectId, Q));
                                    else
                                    {
                                        if ((templateWeapon.Level - client.Character.Record.Level) <= 10)
                                        {
                                            m_drops.Add(new DroppedItem(item.ObjectId, Q));
                                        }
                                    }

                                }
                            }
                        }
                        else
                            alreadyDropped.Quantity++;
                    }
                }
                #endregion
            }

            #endregion

            client.Character.AddKamas((int)FightLoot.kamas);
            var weapon_dropped = 0;
            var item_dropped = 0;
            foreach (var item in m_drops)
            {
                ItemRecord template = ItemRecord.GetItem(item.GID);
                int[] TypeInventoryId = new int[] { 11, 9, 10, 1, 17, 16, 23, 82, 151, 18 };
                if (template != null)
                {
                    if (ArrayUtils.InArray(TypeInventoryId, template.TypeId))
                    {
                        if (item_dropped < 2)
                        {
                            if (client.Character.Record.Level >= template.Level)
                            {
                                FightLoot.objects.Add(item.GID);
                                FightLoot.objects.Add(1);
                                client.Character.Inventory.Add(item.GID, 1, false, false);
                                item_dropped++;
                            }
                            else
                            {
                                if ((template.Level - client.Character.Record.Level) <= 2)
                                {
                                    FightLoot.objects.Add(item.GID);
                                    FightLoot.objects.Add(1);
                                    client.Character.Inventory.Add(item.GID, 1, false, false);
                                    item_dropped++;
                                }
                            }
                        }
                    }
                    else
                    {
                        FightLoot.objects.Add(item.GID);
                        FightLoot.objects.Add(1);
                        client.Character.Inventory.Add(item.GID, 1, false, false);
                    }
                }
                else if (weapon_dropped == 0)
                {
                    FightLoot.objects.Add(item.GID);
                    FightLoot.objects.Add(1);
                    client.Character.Inventory.AddWeapon(item.GID, 1, false, false);
                    weapon_dropped++;
                }
            }

            #region Experience Provider
            List<MonsterData> monsters = new List<MonsterData>();
            foreach (var monster in pvmfight.MonsterGroup.Monsters)
            {
                var grade = MonsterRecord.GetMonster(monster.MonsterId).GetGrade(monster.ActualGrade);
                monsters.Add(new MonsterData(grade.Level, (int)grade.GradeXp));
            }
            var team = Fighter.Team.GetFighters().FindAll(x => x is CharacterFighter).ConvertAll<CharacterFighter>(x => (CharacterFighter)x); ;
            ExperienceFormulas formulas = new ExperienceFormulas();
            formulas.InitXpFormula(new PlayerData(client.Character.Record.Level, client.Character.CharacterStatsRecord.Wisdom, client.Character.Record.DeathMaxLevel), monsters, team.ConvertAll<GroupMemberData>(x => new GroupMemberData(x.Client.Character.Record.Level, false)), this.Fighter.Fight, pvmfight.MonsterGroup.AgeBonus);
            var expWon = this.CheckExpBonusPrism(formulas._xpSolo, Fighter.Fight, client);
            client.Character.AddXp((ulong)expWon);
            PlayerLevel = client.Character.Record.Level;
            var expdatas = new FightResultExperienceData(true, true, true, true, false, false, false, client.Character.Record.Exp, ExperienceRecord.GetExperienceForLevel(client.Character.Record.Level), ExperienceRecord.GetExperienceForLevel((uint)client.Character.Record.Level + 1), (int)formulas._xpSolo, 0, 0, 0);
            AdditionalDatas.Add(expdatas);
            #endregion

            client.Character.Inventory.Refresh();
            client.Character.RefreshShortcuts();


        }
        public uint GetQuantityDropMax()
        {
            uint dropMax = 2;
            if (Fighter.FighterStats.Stats.Prospecting >= 125)
                dropMax = 5;
            if (Fighter.FighterStats.Stats.Prospecting >= 150)
                dropMax = 10;
            if (Fighter.FighterStats.Stats.Prospecting >= 175)
                dropMax = 15;
            if (Fighter.FighterStats.Stats.Prospecting >= 200)
                dropMax = 20;
            if (Fighter.FighterStats.Stats.Prospecting >= 300)
                dropMax = 30;
            return dropMax;
        }
    }

}
