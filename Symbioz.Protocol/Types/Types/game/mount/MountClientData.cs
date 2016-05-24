


















// Generated on 06/04/2015 18:45:36
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Types
{

public class MountClientData
{

public const short Id = 178;
public virtual short TypeId
{
    get { return Id; }
}

public bool sex;
        public bool isRideable;
        public bool isWild;
        public bool isFecondationReady;
        public double id;
        public uint model;
        public IEnumerable<int> ancestor;
        public IEnumerable<int> behaviors;
        public string name;
        public int ownerId;
        public ulong experience;
        public ulong experienceForLevel;
        public double experienceForNextLevel;
        public sbyte level;
        public uint maxPods;
        public uint stamina;
        public uint staminaMax;
        public uint maturity;
        public uint maturityForAdult;
        public uint energy;
        public uint energyMax;
        public int serenity;
        public int aggressivityMax;
        public uint serenityMax;
        public uint love;
        public uint loveMax;
        public int fecondationTime;
        public int boostLimiter;
        public double boostMax;
        public int reproductionCount;
        public uint reproductionCountMax;
        public IEnumerable<Types.ObjectEffectInteger> effectList;
        

public MountClientData()
{
}

public MountClientData(bool sex, bool isRideable, bool isWild, bool isFecondationReady, double id, uint model, IEnumerable<int> ancestor, IEnumerable<int> behaviors, string name, int ownerId, ulong experience, ulong experienceForLevel, double experienceForNextLevel, sbyte level, uint maxPods, uint stamina, uint staminaMax, uint maturity, uint maturityForAdult, uint energy, uint energyMax, int serenity, int aggressivityMax, uint serenityMax, uint love, uint loveMax, int fecondationTime, int boostLimiter, double boostMax, int reproductionCount, uint reproductionCountMax, IEnumerable<Types.ObjectEffectInteger> effectList)
        {
            this.sex = sex;
            this.isRideable = isRideable;
            this.isWild = isWild;
            this.isFecondationReady = isFecondationReady;
            this.id = id;
            this.model = model;
            this.ancestor = ancestor;
            this.behaviors = behaviors;
            this.name = name;
            this.ownerId = ownerId;
            this.experience = experience;
            this.experienceForLevel = experienceForLevel;
            this.experienceForNextLevel = experienceForNextLevel;
            this.level = level;
            this.maxPods = maxPods;
            this.stamina = stamina;
            this.staminaMax = staminaMax;
            this.maturity = maturity;
            this.maturityForAdult = maturityForAdult;
            this.energy = energy;
            this.energyMax = energyMax;
            this.serenity = serenity;
            this.aggressivityMax = aggressivityMax;
            this.serenityMax = serenityMax;
            this.love = love;
            this.loveMax = loveMax;
            this.fecondationTime = fecondationTime;
            this.boostLimiter = boostLimiter;
            this.boostMax = boostMax;
            this.reproductionCount = reproductionCount;
            this.reproductionCountMax = reproductionCountMax;
            this.effectList = effectList;
        }
        

public virtual void Serialize(ICustomDataOutput writer)
{

byte flag1 = 0;
            flag1 = BooleanByteWrapper.SetFlag(flag1, 0, sex);
            flag1 = BooleanByteWrapper.SetFlag(flag1, 1, isRideable);
            flag1 = BooleanByteWrapper.SetFlag(flag1, 2, isWild);
            flag1 = BooleanByteWrapper.SetFlag(flag1, 3, isFecondationReady);
            writer.WriteByte(flag1);
            writer.WriteDouble(id);
            writer.WriteVarUhInt(model);
            writer.WriteUShort((ushort)ancestor.Count());
            foreach (var entry in ancestor)
            {
                 writer.WriteInt(entry);
            }
            writer.WriteUShort((ushort)behaviors.Count());
            foreach (var entry in behaviors)
            {
                 writer.WriteInt(entry);
            }
            writer.WriteUTF(name);
            writer.WriteInt(ownerId);
            writer.WriteVarUhLong(experience);
            writer.WriteVarUhLong(experienceForLevel);
            writer.WriteDouble(experienceForNextLevel);
            writer.WriteSByte(level);
            writer.WriteVarUhInt(maxPods);
            writer.WriteVarUhInt(stamina);
            writer.WriteVarUhInt(staminaMax);
            writer.WriteVarUhInt(maturity);
            writer.WriteVarUhInt(maturityForAdult);
            writer.WriteVarUhInt(energy);
            writer.WriteVarUhInt(energyMax);
            writer.WriteInt(serenity);
            writer.WriteInt(aggressivityMax);
            writer.WriteVarUhInt(serenityMax);
            writer.WriteVarUhInt(love);
            writer.WriteVarUhInt(loveMax);
            writer.WriteInt(fecondationTime);
            writer.WriteInt(boostLimiter);
            writer.WriteDouble(boostMax);
            writer.WriteInt(reproductionCount);
            writer.WriteVarUhInt(reproductionCountMax);
            writer.WriteUShort((ushort)effectList.Count());
            foreach (var entry in effectList)
            {
                 entry.Serialize(writer);
            }
            

}

public virtual void Deserialize(ICustomDataInput reader)
{

byte flag1 = reader.ReadByte();
            sex = BooleanByteWrapper.GetFlag(flag1, 0);
            isRideable = BooleanByteWrapper.GetFlag(flag1, 1);
            isWild = BooleanByteWrapper.GetFlag(flag1, 2);
            isFecondationReady = BooleanByteWrapper.GetFlag(flag1, 3);
            id = reader.ReadDouble();
            if ((id < -9.007199254740992E15) || (id > 9.007199254740992E15))
                throw new Exception("Forbidden value on id = " + id + ", it doesn't respect the following condition : (id < -9.007199254740992E15) || (id > 9.007199254740992E15)");
            model = reader.ReadVarUhInt();
            if (model < 0)
                throw new Exception("Forbidden value on model = " + model + ", it doesn't respect the following condition : model < 0");
            var limit = reader.ReadUShort();
            ancestor = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                 (ancestor as int[])[i] = reader.ReadInt();
            }
            limit = reader.ReadUShort();
            behaviors = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                 (behaviors as int[])[i] = reader.ReadInt();
            }
            name = reader.ReadUTF();
            ownerId = reader.ReadInt();
            if (ownerId < 0)
                throw new Exception("Forbidden value on ownerId = " + ownerId + ", it doesn't respect the following condition : ownerId < 0");
            experience = reader.ReadVarUhLong();
            if ((experience < 0) || (experience > 9.007199254740992E15))
                throw new Exception("Forbidden value on experience = " + experience + ", it doesn't respect the following condition : (experience < 0) || (experience > 9.007199254740992E15)");
            experienceForLevel = reader.ReadVarUhLong();
            if ((experienceForLevel < 0) || (experienceForLevel > 9.007199254740992E15))
                throw new Exception("Forbidden value on experienceForLevel = " + experienceForLevel + ", it doesn't respect the following condition : (experienceForLevel < 0) || (experienceForLevel > 9.007199254740992E15)");
            experienceForNextLevel = reader.ReadDouble();
            if ((experienceForNextLevel < -9.007199254740992E15) || (experienceForNextLevel > 9.007199254740992E15))
                throw new Exception("Forbidden value on experienceForNextLevel = " + experienceForNextLevel + ", it doesn't respect the following condition : (experienceForNextLevel < -9.007199254740992E15) || (experienceForNextLevel > 9.007199254740992E15)");
            level = reader.ReadSByte();
            if (level < 0)
                throw new Exception("Forbidden value on level = " + level + ", it doesn't respect the following condition : level < 0");
            maxPods = reader.ReadVarUhInt();
            if (maxPods < 0)
                throw new Exception("Forbidden value on maxPods = " + maxPods + ", it doesn't respect the following condition : maxPods < 0");
            stamina = reader.ReadVarUhInt();
            if (stamina < 0)
                throw new Exception("Forbidden value on stamina = " + stamina + ", it doesn't respect the following condition : stamina < 0");
            staminaMax = reader.ReadVarUhInt();
            if (staminaMax < 0)
                throw new Exception("Forbidden value on staminaMax = " + staminaMax + ", it doesn't respect the following condition : staminaMax < 0");
            maturity = reader.ReadVarUhInt();
            if (maturity < 0)
                throw new Exception("Forbidden value on maturity = " + maturity + ", it doesn't respect the following condition : maturity < 0");
            maturityForAdult = reader.ReadVarUhInt();
            if (maturityForAdult < 0)
                throw new Exception("Forbidden value on maturityForAdult = " + maturityForAdult + ", it doesn't respect the following condition : maturityForAdult < 0");
            energy = reader.ReadVarUhInt();
            if (energy < 0)
                throw new Exception("Forbidden value on energy = " + energy + ", it doesn't respect the following condition : energy < 0");
            energyMax = reader.ReadVarUhInt();
            if (energyMax < 0)
                throw new Exception("Forbidden value on energyMax = " + energyMax + ", it doesn't respect the following condition : energyMax < 0");
            serenity = reader.ReadInt();
            aggressivityMax = reader.ReadInt();
            serenityMax = reader.ReadVarUhInt();
            if (serenityMax < 0)
                throw new Exception("Forbidden value on serenityMax = " + serenityMax + ", it doesn't respect the following condition : serenityMax < 0");
            love = reader.ReadVarUhInt();
            if (love < 0)
                throw new Exception("Forbidden value on love = " + love + ", it doesn't respect the following condition : love < 0");
            loveMax = reader.ReadVarUhInt();
            if (loveMax < 0)
                throw new Exception("Forbidden value on loveMax = " + loveMax + ", it doesn't respect the following condition : loveMax < 0");
            fecondationTime = reader.ReadInt();
            boostLimiter = reader.ReadInt();
            if (boostLimiter < 0)
                throw new Exception("Forbidden value on boostLimiter = " + boostLimiter + ", it doesn't respect the following condition : boostLimiter < 0");
            boostMax = reader.ReadDouble();
            if ((boostMax < -9.007199254740992E15) || (boostMax > 9.007199254740992E15))
                throw new Exception("Forbidden value on boostMax = " + boostMax + ", it doesn't respect the following condition : (boostMax < -9.007199254740992E15) || (boostMax > 9.007199254740992E15)");
            reproductionCount = reader.ReadInt();
            reproductionCountMax = reader.ReadVarUhInt();
            if (reproductionCountMax < 0)
                throw new Exception("Forbidden value on reproductionCountMax = " + reproductionCountMax + ", it doesn't respect the following condition : reproductionCountMax < 0");
            limit = reader.ReadUShort();
            effectList = new Types.ObjectEffectInteger[limit];
            for (int i = 0; i < limit; i++)
            {
                 (effectList as Types.ObjectEffectInteger[])[i] = new Types.ObjectEffectInteger();
                 (effectList as Types.ObjectEffectInteger[])[i].Deserialize(reader);
            }
            

}


}


}