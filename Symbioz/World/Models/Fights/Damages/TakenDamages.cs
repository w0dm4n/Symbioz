using Symbioz.DofusProtocol.Messages;
using Symbioz.World.Models.Fights.Damages;
using Symbioz.World.Models.Fights.Fighters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Models.Fights.Damages
{
    public class TakenDamages
    {
        public short Delta { get; set; }

        public ElementType ElementType { get; set; }

        public TakenDamages(short delta, ElementType elementtype)
        {
            this.Delta = delta;
            this.ElementType = elementtype;
        }
        public short GetDeltaPercentage(sbyte percentage)
        {
           return (short)((double)Delta * ((double)percentage / 100.0));
        }
        /// <summary>
        ///  Dégâts subis =dégâts totaux - résistance fixe - (dégâts totaux / 100 / pourcentage de résistance) 
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public void EvaluateWithResistances(Fighter source,Fighter target,bool pvp)
        {
            if (source == null)
                return;
            short elementBonus = 0;//statsC
            short elementReduction = 0;//resfT
            double elementResistPercent = 0;//respT
            double mulT = 1;
            double a = 1;//calcul
            double i = 0;//bonus maitrise
            double j = 100;//bonus classe
            double domC = 0, perdomC = 0;
            int multiplier = 0;
                domC = source.FighterStats.Stats.AllDamagesBonus;
                perdomC = source.FighterStats.Stats.AllDamagesBonusPercent;
            switch (ElementType)
            {
                case ElementType.Earth:
                    elementBonus = source.FighterStats.Stats.EarthDamageBonus;
                    elementReduction = (short)(target.FighterStats.Stats.EarthReduction + (pvp ? target.FighterStats.Stats.PvPEarthReduction : 0));
                    elementResistPercent = target.FighterStats.Stats.EarthResistPercent + (pvp ? target.FighterStats.Stats.PvPEarthResistPercent : 0);
                    break;
                case ElementType.Air:
                    elementBonus = source.FighterStats.Stats.AirDamageBonus;
                    elementReduction = (short)(target.FighterStats.Stats.AirReduction + (pvp ? target.FighterStats.Stats.PvPAirReduction : 0));
                    elementResistPercent = target.FighterStats.Stats.AirResistPercent + (pvp? target.FighterStats.Stats.PvPAirResistPercent : 0);
                    break;
                case ElementType.Water:
                    elementBonus = source.FighterStats.Stats.WaterDamageBonus;
                    elementReduction = (short)(target.FighterStats.Stats.WaterReduction + (pvp ? target.FighterStats.Stats.PvPWaterReduction : 0));
                    elementResistPercent = target.FighterStats.Stats.WaterResistPercent + (pvp ? target.FighterStats.Stats.PvPWaterResistPercent : 0);
                    break;
                case ElementType.Fire:
                    elementBonus = source.FighterStats.Stats.FireDamageBonus;
                    elementReduction = (short)(target.FighterStats.Stats.FireReduction  + (pvp ? target.FighterStats.Stats.PvPFireReduction : 0));
                    elementResistPercent = target.FighterStats.Stats.FireResistPercent + (pvp ? target.FighterStats.Stats.PvPFireResistPercent : 0);
                    break;
                case ElementType.Neutral:
                    elementBonus = source.FighterStats.Stats.NeutralDamageBonus;
                    elementReduction = (short)(target.FighterStats.Stats.NeutralReduction + (pvp? target.FighterStats.Stats.PvPNeutralReduction : 0));
                    elementResistPercent = target.FighterStats.Stats.NeutralResistPercent + (pvp ? target.FighterStats.Stats.PvPNeutralResistPercent : 0);
                    break;
                case ElementType.Push:
                    elementBonus = source.FighterStats.Stats.PushDamageBonus;
                    elementReduction = (short)(target.FighterStats.Stats.PushDamageReduction);
                    elementResistPercent = 0;
                    break;
                default:
                    break;
            }
            //Delta += elementBonus;
            if (source.UsingWeapon)
            {
                i = source.FighterStats.Stats.WeaponDamagesBonusPercent;
                a = (((100 + (i / 5)) / 100) * (100 / 100));//lavant dernier 100 c'est bonus classe sur un cac mais pas fait pour le moment
            }
            //Delta += (short)((source.FighterStats.Stats.WeaponDamagesBonusPercent / 100) * Delta);

            float lok = ((100 + elementBonus) / 100);
            if (lok < 1)
                lok = 1;
            Delta = (short)(mulT * a * (Delta * lok) + domC);//dégats bruts
            if (elementResistPercent != 0)
                Delta = (short)(Delta - elementReduction - (elementResistPercent/ 100 * Delta));
            else
                Delta -= elementReduction;
            if (target.FighterStats.Stats.GlobalDamageReduction > 0)
            {
                Delta -= target.FighterStats.Stats.GlobalDamageReduction;
                target.Fight.Send(new GameActionFightReduceDamagesMessage(0, source.ContextualId, target.ContextualId, (uint)target.FighterStats.Stats.GlobalDamageReduction));
            }
            if (Delta <= 0)
                Delta = 0;
        }

        public int EvaluatePushResistances(Fighter source, Fighter target, bool pvp)
        {
            if (source == null)
                return (0);
            short elementBonus = 0;
            short elementReduction = 0;
            double elementResistPercent = 0;

            elementBonus = source.FighterStats.Stats.PushDamageBonus;
            elementReduction = (short)(target.FighterStats.Stats.PushDamageReduction);
            elementResistPercent = 0;
            Delta += elementBonus;
            if (source.UsingWeapon)
                Delta += (short)((source.FighterStats.Stats.WeaponDamagesBonusPercent / 100) * Delta);
            if (elementResistPercent != 0)
                Delta = (short)(Delta - elementReduction - (elementResistPercent / 100 * Delta));
            else
                Delta -= elementReduction;
            if (target.FighterStats.Stats.GlobalDamageReduction > 0)
            {
                return (target.FighterStats.Stats.GlobalDamageReduction);
            }
            return (0);
        }

    }
}
