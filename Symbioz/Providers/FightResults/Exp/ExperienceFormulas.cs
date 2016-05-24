using Symbioz.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.Providers.FightResults.Exp
{

    class ExperienceFormulas
    {
        static double[] XP_GROUP = new double[] { 1, 1.1, 1.5, 2.3, 3.1, 3.6, 4.2, 4.7 };

        public double _xpSolo { get; set; }
        public double _xpGroup { get; set; }

        private double truncate(double val)
        {

            double multiplier = Math.Pow(10, 0);
            double truncatedVal = (val * multiplier);
            return (truncatedVal / multiplier);
        }

        public void InitXpFormula(PlayerData pPlayerData, List<MonsterData> pMonstersList, List<GroupMemberData> pMembersList, double StarsBonus = 0)
        {
            double lvlPlayers = 0;
            double lvlMaxGroup = 0;
            double totalPlayerForBonusGroup = 0;
            double coeffDiffLvlGroup;
            double ratioXpMontureSolo;
            double ratioXpMontureGroup;
            double ratioXpGuildSolo;
            double ratioXpGuildGroup;
            double xpAlliancePrismBonus;
            double xpBase = 0;
            double maxLvlMonster = 0;
            double lvlMonsters = 0;

            foreach (var mob in pMonstersList)
            {
                xpBase += mob.xp;
                lvlMonsters += mob.level;
                if (mob.level > maxLvlMonster)
                {
                    maxLvlMonster = mob.level;
                }
            }
            lvlPlayers = 0;
            lvlMaxGroup = 0;
            totalPlayerForBonusGroup = 0;

            foreach (var member in pMembersList)
            {
                lvlPlayers += member.level;
                if (member.level > lvlMaxGroup)
                {
                    lvlMaxGroup = member.level;
                }
            }
            foreach (var member in pMembersList)
            {
                if ((!member.companion) && (member.level >= lvlMaxGroup / 3))
                {
                    totalPlayerForBonusGroup++;
                }
            }
            coeffDiffLvlGroup = 1;
            if (lvlPlayers - 5 > lvlMonsters)
            {
                coeffDiffLvlGroup = (lvlMonsters / lvlPlayers);
            }
            else if (lvlPlayers + 10 < lvlMonsters)
            {
                coeffDiffLvlGroup = (lvlPlayers + 10) / lvlMonsters;
            }
            double coeffDiffLvlSolo = 1;
            if (pPlayerData.level - 5 > lvlMonsters)
            {
                coeffDiffLvlSolo = (lvlMonsters / pPlayerData.level);
            }
            else if (pPlayerData.level + 10 < lvlMonsters)
            {
                coeffDiffLvlSolo = ((pPlayerData.level + 10) / lvlMonsters);
            }
            double v = (Math.Min(pPlayerData.level, this.truncate(2.5 * maxLvlMonster)));
            double xpLimitMaxLvlSolo = v / pPlayerData.level * 100;
            double xpLimitMaxLvlGroup = (v / lvlPlayers * 10);
            double xpGroupAlone = this.truncate(xpBase * XP_GROUP[0] * coeffDiffLvlSolo);
            if (totalPlayerForBonusGroup == 0)
            {
                totalPlayerForBonusGroup = 1;
            }
            double xpGroup = this.truncate(xpBase * XP_GROUP[(int)(totalPlayerForBonusGroup - 1)] * coeffDiffLvlGroup);
            double xpNoSagesseAlone = this.truncate(xpLimitMaxLvlSolo / 100 * xpGroupAlone);
            double xpNoSagesseGroup = this.truncate(xpLimitMaxLvlGroup / 100 * xpGroup);
            double reelStarBonus = StarsBonus <= 0 ? 1 : 1 + StarsBonus / 100;

            var o = this.truncate((xpNoSagesseAlone * (100 + pPlayerData.wisdom) / 100));
            double xpTotalOnePlayer = this.truncate((o * reelStarBonus));
            var m = this.truncate((xpNoSagesseGroup * (100 + pPlayerData.wisdom) / 100));
            double xpTotalGroup = this.truncate(m * reelStarBonus);
            double xpBonus = 1 + pPlayerData.xpBonusPercent / 100;
            double tmpSolo = xpTotalOnePlayer;
            double tmpGroup = xpTotalGroup;

            if (pPlayerData.xpRatioMount > 0)
            {
                ratioXpMontureSolo = tmpSolo * pPlayerData.xpRatioMount / 100;
                ratioXpMontureGroup = tmpGroup * pPlayerData.xpRatioMount / 100;
                tmpSolo = this.truncate((tmpSolo - ratioXpMontureSolo));
                tmpGroup = this.truncate((tmpGroup - ratioXpMontureGroup));
            }
            tmpSolo = tmpSolo * xpBonus;
            tmpGroup = tmpGroup * xpBonus;

            if (pPlayerData.xpGuildGivenPercent > 0)
            {
                ratioXpGuildSolo = tmpSolo * pPlayerData.xpGuildGivenPercent / 100;
                ratioXpGuildGroup = tmpGroup * pPlayerData.xpGuildGivenPercent / 100;
                tmpSolo = tmpSolo - ratioXpGuildSolo;
                tmpGroup = tmpGroup - ratioXpGuildGroup;
            }
            if (pPlayerData.xpAlliancePrismBonusPercent > 0)
            {
                xpAlliancePrismBonus = 1 + pPlayerData.xpAlliancePrismBonusPercent / 100;
                tmpSolo = tmpSolo * xpAlliancePrismBonus;
                tmpGroup = tmpGroup * xpAlliancePrismBonus;
            }
            xpTotalOnePlayer = this.truncate(tmpSolo);
            xpTotalGroup = this.truncate(tmpGroup);
            this._xpSolo = xpTotalOnePlayer * ConfigurationManager.Instance.ExperienceRatio;
            this._xpGroup = xpTotalGroup;
        }
    }
}
