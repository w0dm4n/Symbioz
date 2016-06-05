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
        public FightResultPlayer(CharacterFighter fighter, TeamColorEnum winner)
            : base(fighter, winner)
        {
            this.PlayerLevel = fighter.Client.Character.Record.Level;
            WorldClient client = (Fighter as CharacterFighter).Client;
            if (fighter.Fight is FightPvM && winner == fighter.Team.TeamColor)
            {
                GeneratePVMLoot();
                if (ConfigurationManager.Instance.ServerId == 22)
                {
                    // heroique drop item
                }
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
                        client.Character.Reply("Vous êtes mort !", false, false);
                    }
                }
            }
            if (fighter.Fight is FightArena && winner == fighter.Team.TeamColor)
            {
                GenerateArenaLoot();
            }

            if (fighter.Fight is FightAgression && winner == fighter.Team.TeamColor)//WINNER
            {
                // add honor
                if (ConfigurationManager.Instance.ServerId == 22)
                {
                    // heroique drop item
                }
            }
            else if (fighter.Fight is FightAgression && winner != fighter.Team.TeamColor && ConfigurationManager.Instance.ServerId == 22)//LOOSER
            {
                client.Character.Record.DeathCount++;
                if (client.Character.Record.DeathMaxLevel < client.Character.Record.Level)
                    client.Character.Record.DeathMaxLevel = client.Character.Record.Level;
                client.Character.Record.Energy = 0;
                client.Character.Look = ContextActorLook.Parse("{24}");
            }
            if (fighter.disconnect)
            {
                CharactersDisconnected.remove(client.Character.Record.Name);
                client.Character.AddElement(client.Character.Record);
                WorldServer.Instance.WorldClients.Remove(client);
            }
        }
        public override FightResultListEntry GetEntry()
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

        public static bool InArray(int[] array, int type)
        {
            foreach (var value in array)
                if (value == type)
                    return (true);
            return (false);
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
                int prospectingSum = charactersFighters.Sum((CharacterFighter entry) => entry.Client.Character.StatsRecord.Prospecting);

                foreach (var item in template.Drops.FindAll(x => x.ProspectingLock <= prospectingSum))
                {
                    int D = random.Next(0, 201);
                    double dropchancePercent = (((item.GetDropRate(monster.ActualGrade) + pvmfight.MonsterGroup.AgeBonus / 5 + client.Character.StatsRecord.Prospecting / 100) / 3) * ConfigurationManager.Instance.ItemsDropRatio);

                    if (D <= dropchancePercent)
                    {
                        var alreadyDropped = m_drops.FirstOrDefault(x => x.GID == item.ObjectId);
                        if (alreadyDropped == null)
                        {
                            uint dropMax = GetQuantityDropMax();

                            uint Q = (uint)random.Next(1, (int)(dropMax + 1));
                            if (Q > item.Count)
                                Q = 1;
                            m_drops.Add(new DroppedItem(item.ObjectId, Q));
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
                    if (InArray(TypeInventoryId, template.TypeId))
                    {
                        if (item_dropped < 2)
                        {
                            FightLoot.objects.Add(item.GID);
                            FightLoot.objects.Add(1);
                            client.Character.Inventory.Add(item.GID, 1, false, false);
                            item_dropped++;
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
            formulas.InitXpFormula(new PlayerData(client.Character.Record.Level, client.Character.StatsRecord.Wisdom, client.Character.Record.DeathMaxLevel), monsters, team.ConvertAll<GroupMemberData>(x => new GroupMemberData(x.Client.Character.Record.Level, false)), pvmfight.MonsterGroup.AgeBonus);
            if (client.Character.Record.Level >= 200)
                formulas._xpSolo = 0;
            client.Character.AddXp((ulong)formulas._xpSolo);
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
