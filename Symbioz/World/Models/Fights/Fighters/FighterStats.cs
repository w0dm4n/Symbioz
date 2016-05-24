using Symbioz.DofusProtocol.Types;
using Symbioz.Enums;
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

        public StatsRecord RealStats { get; set; }

        public StatsRecord Stats { get; set; }

        public bool Summoned { get; set; }

        public int SummonerId { get; set; }

        public short ApUsed { get { return (short)(RealStats.ActionPoints - Stats.ActionPoints); } }

        public short MpUsed { get { return (short)(RealStats.MovementPoints - Stats.MovementPoints); } }

        public short MPPercentage { get { return (short)(((double)Stats.MovementPoints / (double)RealStats.MovementPoints) * 100); } }

        public short APPercentage { get { return (short)(((double)Stats.ActionPoints / (double)RealStats.ActionPoints) * 100); } }

        public short LifePercentage { get { return (short)(((double)Stats.LifePoints / (double)RealStats.LifePoints) * 100); } }

        public short ErosionPercentage { get; set; }

        public int ErodedLife { get; set; }

        public FighterStats(StatsRecord record, bool summoned = false, int summonerid = 0)
        {
            this.RealStats = record.Clone();
            this.Stats = RealStats.Clone();
            this.ShieldPoints = 0;
            this.ErosionPercentage = 0;
            this.ErodedLife = 0;
            this.Summoned = summoned;
            this.SummonerId = summonerid;
        }
        public void OnTurnEnded()
        {
            this.Stats.MovementPoints = RealStats.MovementPoints;
            this.Stats.ActionPoints = RealStats.ActionPoints;
        }
        public GameFightMinimalStats GetMinimalStats()
        {
            return new GameFightMinimalStats((uint)Stats.LifePoints,(uint)RealStats.LifePoints, (uint)(RealStats.LifePoints + ErodedLife), (uint)Stats.AllDamagesBonus,
                (uint)ShieldPoints, Stats.ActionPoints, RealStats.ActionPoints, Stats.MovementPoints, RealStats.MovementPoints,
                SummonerId, Summoned, Stats.NeutralResistPercent, Stats.EarthResistPercent, Stats.WaterResistPercent, Stats.AirResistPercent, Stats.FireResistPercent,
                Stats.NeutralReduction, Stats.EarthReduction, Stats.WaterReduction, Stats.AirReduction, Stats.FireReduction,
                Stats.CriticalDamageReduction, Stats.PushDamageReduction, (ushort)Stats.DodgePA, (ushort)Stats.DodgePM, 0, 0, (sbyte)InvisiblityState);
        }
        public CharacterCharacteristicsInformations GetCharacterCharacteristics(Character character)
        {
            return StatsRecord.GetCharacterCharacteristics(Stats, character);
        }
    }
}
