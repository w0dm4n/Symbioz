


















// Generated on 06/04/2015 18:45:25
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Types
{

    public class GameFightMinimalStats
    {

        public const short Id = 31;
        public virtual short TypeId
        {
            get { return Id; }
        }

        public uint lifePoints;
        public uint maxLifePoints;
        public uint baseMaxLifePoints;
        public uint permanentDamagePercent;
        public uint shieldPoints;
        public short actionPoints;
        public short maxActionPoints;
        public short movementPoints;
        public short maxMovementPoints;
        public int summoner;
        public bool summoned;
        public short neutralElementResistPercent;
        public short earthElementResistPercent;
        public short waterElementResistPercent;
        public short airElementResistPercent;
        public short fireElementResistPercent;
        public short neutralElementReduction;
        public short earthElementReduction;
        public short waterElementReduction;
        public short airElementReduction;
        public short fireElementReduction;
        public short criticalDamageFixedResist;
        public short pushDamageFixedResist;
        public ushort dodgePALostProbability;
        public ushort dodgePMLostProbability;
        public short tackleBlock;
        public short tackleEvade;
        public sbyte invisibilityState;


        public GameFightMinimalStats()
        {
        }

        public GameFightMinimalStats(uint lifePoints, uint maxLifePoints, uint baseMaxLifePoints, uint permanentDamagePercent, uint shieldPoints, short actionPoints, short maxActionPoints, short movementPoints, short maxMovementPoints, int summoner, bool summoned, short neutralElementResistPercent, short earthElementResistPercent, short waterElementResistPercent, short airElementResistPercent, short fireElementResistPercent, short neutralElementReduction, short earthElementReduction, short waterElementReduction, short airElementReduction, short fireElementReduction, short criticalDamageFixedResist, short pushDamageFixedResist, ushort dodgePALostProbability, ushort dodgePMLostProbability, short tackleBlock, short tackleEvade, sbyte invisibilityState)
        {
            this.lifePoints = lifePoints;
            this.maxLifePoints = maxLifePoints;
            this.baseMaxLifePoints = baseMaxLifePoints;
            this.permanentDamagePercent = permanentDamagePercent;
            this.shieldPoints = shieldPoints;
            this.actionPoints = actionPoints;
            this.maxActionPoints = maxActionPoints;
            this.movementPoints = movementPoints;
            this.maxMovementPoints = maxMovementPoints;
            this.summoner = summoner;
            this.summoned = summoned;
            this.neutralElementResistPercent = neutralElementResistPercent;
            this.earthElementResistPercent = earthElementResistPercent;
            this.waterElementResistPercent = waterElementResistPercent;
            this.airElementResistPercent = airElementResistPercent;
            this.fireElementResistPercent = fireElementResistPercent;
            this.neutralElementReduction = neutralElementReduction;
            this.earthElementReduction = earthElementReduction;
            this.waterElementReduction = waterElementReduction;
            this.airElementReduction = airElementReduction;
            this.fireElementReduction = fireElementReduction;
            this.criticalDamageFixedResist = criticalDamageFixedResist;
            this.pushDamageFixedResist = pushDamageFixedResist;
            this.dodgePALostProbability = dodgePALostProbability;
            this.dodgePMLostProbability = dodgePMLostProbability;
            this.tackleBlock = tackleBlock;
            this.tackleEvade = tackleEvade;
            this.invisibilityState = invisibilityState;
        }


        public virtual void Serialize(ICustomDataOutput writer)
        {

            writer.WriteVarUhInt(lifePoints);
            writer.WriteVarUhInt(maxLifePoints);
            writer.WriteVarUhInt(baseMaxLifePoints);
            writer.WriteVarUhInt(permanentDamagePercent);
            writer.WriteVarUhInt(shieldPoints);
            writer.WriteVarShort(actionPoints);
            writer.WriteVarShort(maxActionPoints);
            writer.WriteVarShort(movementPoints);
            writer.WriteVarShort(maxMovementPoints);
            writer.WriteInt(summoner);
            writer.WriteBoolean(summoned);
            writer.WriteVarShort(neutralElementResistPercent);
            writer.WriteVarShort(earthElementResistPercent);
            writer.WriteVarShort(waterElementResistPercent);
            writer.WriteVarShort(airElementResistPercent);
            writer.WriteVarShort(fireElementResistPercent);
            writer.WriteVarShort(neutralElementReduction);
            writer.WriteVarShort(earthElementReduction);
            writer.WriteVarShort(waterElementReduction);
            writer.WriteVarShort(airElementReduction);
            writer.WriteVarShort(fireElementReduction);
            writer.WriteVarShort(criticalDamageFixedResist);
            writer.WriteVarShort(pushDamageFixedResist);
            writer.WriteVarUhShort(dodgePALostProbability);
            writer.WriteVarUhShort(dodgePMLostProbability);
            writer.WriteVarShort(tackleBlock);
            writer.WriteVarShort(tackleEvade);
            writer.WriteSByte(invisibilityState);


        }
        public GameFightMinimalStats Clone()
        {
            return new GameFightMinimalStats(lifePoints, maxLifePoints, baseMaxLifePoints, permanentDamagePercent, shieldPoints, actionPoints, maxActionPoints, movementPoints, maxMovementPoints,summoner, summoned, neutralElementResistPercent, earthElementResistPercent, waterElementResistPercent, airElementResistPercent,
                fireElementResistPercent, neutralElementResistPercent, earthElementReduction, waterElementReduction, airElementReduction, fireElementReduction,
                criticalDamageFixedResist, pushDamageFixedResist, dodgePALostProbability, dodgePMLostProbability, tackleBlock, tackleEvade, invisibilityState);
        }
        public virtual void Deserialize(ICustomDataInput reader)
        {

            lifePoints = reader.ReadVarUhInt();
            if (lifePoints < 0)
                throw new Exception("Forbidden value on lifePoints = " + lifePoints + ", it doesn't respect the following condition : lifePoints < 0");
            maxLifePoints = reader.ReadVarUhInt();
            if (maxLifePoints < 0)
                throw new Exception("Forbidden value on maxLifePoints = " + maxLifePoints + ", it doesn't respect the following condition : maxLifePoints < 0");
            baseMaxLifePoints = reader.ReadVarUhInt();
            if (baseMaxLifePoints < 0)
                throw new Exception("Forbidden value on baseMaxLifePoints = " + baseMaxLifePoints + ", it doesn't respect the following condition : baseMaxLifePoints < 0");
            permanentDamagePercent = reader.ReadVarUhInt();
            if (permanentDamagePercent < 0)
                throw new Exception("Forbidden value on permanentDamagePercent = " + permanentDamagePercent + ", it doesn't respect the following condition : permanentDamagePercent < 0");
            shieldPoints = reader.ReadVarUhInt();
            if (shieldPoints < 0)
                throw new Exception("Forbidden value on shieldPoints = " + shieldPoints + ", it doesn't respect the following condition : shieldPoints < 0");
            actionPoints = reader.ReadVarShort();
            maxActionPoints = reader.ReadVarShort();
            movementPoints = reader.ReadVarShort();
            maxMovementPoints = reader.ReadVarShort();
            summoner = reader.ReadInt();
            summoned = reader.ReadBoolean();
            neutralElementResistPercent = reader.ReadVarShort();
            earthElementResistPercent = reader.ReadVarShort();
            waterElementResistPercent = reader.ReadVarShort();
            airElementResistPercent = reader.ReadVarShort();
            fireElementResistPercent = reader.ReadVarShort();
            neutralElementReduction = reader.ReadVarShort();
            earthElementReduction = reader.ReadVarShort();
            waterElementReduction = reader.ReadVarShort();
            airElementReduction = reader.ReadVarShort();
            fireElementReduction = reader.ReadVarShort();
            criticalDamageFixedResist = reader.ReadVarShort();
            pushDamageFixedResist = reader.ReadVarShort();
            dodgePALostProbability = reader.ReadVarUhShort();
            if (dodgePALostProbability < 0)
                throw new Exception("Forbidden value on dodgePALostProbability = " + dodgePALostProbability + ", it doesn't respect the following condition : dodgePALostProbability < 0");
            dodgePMLostProbability = reader.ReadVarUhShort();
            if (dodgePMLostProbability < 0)
                throw new Exception("Forbidden value on dodgePMLostProbability = " + dodgePMLostProbability + ", it doesn't respect the following condition : dodgePMLostProbability < 0");
            tackleBlock = reader.ReadVarShort();
            tackleEvade = reader.ReadVarShort();
            invisibilityState = reader.ReadSByte();
            if (invisibilityState < 0)
                throw new Exception("Forbidden value on invisibilityState = " + invisibilityState + ", it doesn't respect the following condition : invisibilityState < 0");


        }


    }


}