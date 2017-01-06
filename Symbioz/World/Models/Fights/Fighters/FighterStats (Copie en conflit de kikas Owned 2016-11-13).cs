using Symbioz.DofusProtocol.Types;
using Symbioz.Enums;
using Symbioz.World.Models.Fights.Fighters;
using Symbioz.World.Records;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Models
{
    public class FighterStats
    {
        public GameActionFightInvisibilityStateEnum InvisiblityState { get; set; }

        public int ShieldPoints { get; set; }

        public CharacterStatsRecord RealStats { get; set; }

        public CharacterStatsRecord Stats { get; set; }

        public bool Summoned { get; set; }

        public int SummonerId { get; set; }

        public short ApUsed { get { return (short)(RealStats.ActionPoints - Stats.ActionPoints); } }

        public short MpUsed { get { return (short)(RealStats.MovementPoints - Stats.MovementPoints); } }

        public short MPPercentage { get { return (short)(((double)Stats.MovementPoints / (double)RealStats.MovementPoints) * 100); } }

        public short APPercentage { get { return (short)(((double)Stats.ActionPoints / (double)RealStats.ActionPoints) * 100); } }

        public short LifePercentage { get { return (short)(((double)Stats.LifePoints / (double)RealStats.LifePoints) * 100); } }

        public short ErosionPercentage { get; set; }



        public int ErodedLife { get; set; }

        public FighterStats(CharacterStatsRecord record, bool summoned = false, int summonerid = 0, BasicStats current = null)
        {
            this.RealStats = record.Clone();
            InitializeLimit();
            this.Stats = RealStats.Clone();
            if (RealStats.ActionPoints > 12)
                RealStats.ActionPoints = 12;
            if (current != null)
                if (current.LifePoints > 0)
                    this.Stats.LifePoints = (short)current.LifePoints;
                else
                    this.Stats.LifePoints = 1;
            this.ShieldPoints = 0;
            this.ErosionPercentage = 0;
            this.ErodedLife = 0;
            this.Summoned = summoned;
            this.SummonerId = summonerid;
        }

        public void InitializeLimit()
        {
            if (RealStats.ActionPoints > 12)
                RealStats.ActionPoints = 12;
            if (RealStats.MovementPoints > 6)
                RealStats.MovementPoints = 6;
        }

        public void OnTurnEnded()
        {
            this.Stats.MovementPoints = RealStats.MovementPoints > 6 ? (short)6 : RealStats.MovementPoints;
            this.Stats.ActionPoints = RealStats.ActionPoints > 12 ? (short)12 : RealStats.ActionPoints;
        }
        public GameFightMinimalStats GetMinimalStats()
        {
            return new GameFightMinimalStats((uint)Stats.LifePoints,(uint)RealStats.LifePoints, (uint)(RealStats.LifePoints + ErodedLife), (uint)Stats.AllDamagesBonus,
                (uint)ShieldPoints, Stats.ActionPoints > 12 ? (short)12 : Stats.ActionPoints, 12, Stats.MovementPoints > 6 ? (short)6 : Stats.MovementPoints, 6,
                SummonerId, Summoned, Stats.NeutralResistPercent, Stats.EarthResistPercent, Stats.WaterResistPercent, Stats.AirResistPercent, Stats.FireResistPercent,
                Stats.NeutralReduction, Stats.EarthReduction, Stats.WaterReduction, Stats.AirReduction, Stats.FireReduction,
                Stats.CriticalDamageReduction, Stats.PushDamageReduction, (ushort)Stats.DodgePA, (ushort)Stats.DodgePM, Stats.TackleBlock, Stats.TackleEvade, (sbyte)InvisiblityState);
        }

        private double GetTacklePercent(Fighter tackler)
        {
            double result;
            if (tackler.FighterStats.Stats.TackleBlock == -2)
            {
                result = 0.0;
            }
            else
            {
                result = (double)(this.Stats.TackleEvade + 2) / (2.0 * (double)( tackler.FighterStats.Stats.TackleBlock + 2));
            }
            return result;
        }

        public CharacterCharacteristicsInformations GetCharacterCharacteristics(Character character)
        {
            return CharacterStatsRecord.GetCharacterCharacteristics(Stats, character);
        }
    }
}
