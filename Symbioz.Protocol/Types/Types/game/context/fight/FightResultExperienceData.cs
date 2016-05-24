


















// Generated on 06/04/2015 18:45:24
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Types
{

public class FightResultExperienceData : FightResultAdditionalData
{

public const short Id = 192;
public override short TypeId
{
    get { return Id; }
}

public bool showExperience;
        public bool showExperienceLevelFloor;
        public bool showExperienceNextLevelFloor;
        public bool showExperienceFightDelta;
        public bool showExperienceForGuild;
        public bool showExperienceForMount;
        public bool isIncarnationExperience;
        public ulong experience;
        public ulong experienceLevelFloor;
        public double experienceNextLevelFloor;
        public int experienceFightDelta;
        public uint experienceForGuild;
        public uint experienceForMount;
        public sbyte rerollExperienceMul;
        

public FightResultExperienceData()
{
}

public FightResultExperienceData(bool showExperience, bool showExperienceLevelFloor, bool showExperienceNextLevelFloor, bool showExperienceFightDelta, bool showExperienceForGuild, bool showExperienceForMount, bool isIncarnationExperience, ulong experience, ulong experienceLevelFloor, double experienceNextLevelFloor, int experienceFightDelta, uint experienceForGuild, uint experienceForMount, sbyte rerollExperienceMul)
        {
            this.showExperience = showExperience;
            this.showExperienceLevelFloor = showExperienceLevelFloor;
            this.showExperienceNextLevelFloor = showExperienceNextLevelFloor;
            this.showExperienceFightDelta = showExperienceFightDelta;
            this.showExperienceForGuild = showExperienceForGuild;
            this.showExperienceForMount = showExperienceForMount;
            this.isIncarnationExperience = isIncarnationExperience;
            this.experience = experience;
            this.experienceLevelFloor = experienceLevelFloor;
            this.experienceNextLevelFloor = experienceNextLevelFloor;
            this.experienceFightDelta = experienceFightDelta;
            this.experienceForGuild = experienceForGuild;
            this.experienceForMount = experienceForMount;
            this.rerollExperienceMul = rerollExperienceMul;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            byte flag1 = 0;
            flag1 = BooleanByteWrapper.SetFlag(flag1, 0, showExperience);
            flag1 = BooleanByteWrapper.SetFlag(flag1, 1, showExperienceLevelFloor);
            flag1 = BooleanByteWrapper.SetFlag(flag1, 2, showExperienceNextLevelFloor);
            flag1 = BooleanByteWrapper.SetFlag(flag1, 3, showExperienceFightDelta);
            flag1 = BooleanByteWrapper.SetFlag(flag1, 4, showExperienceForGuild);
            flag1 = BooleanByteWrapper.SetFlag(flag1, 5, showExperienceForMount);
            flag1 = BooleanByteWrapper.SetFlag(flag1, 6, isIncarnationExperience);
            writer.WriteByte(flag1);
            writer.WriteVarUhLong(experience);
            writer.WriteVarUhLong(experienceLevelFloor);
            writer.WriteDouble(experienceNextLevelFloor);
            writer.WriteVarInt(experienceFightDelta);
            writer.WriteVarUhInt(experienceForGuild);
            writer.WriteVarUhInt(experienceForMount);
            writer.WriteSByte(rerollExperienceMul);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            byte flag1 = reader.ReadByte();
            showExperience = BooleanByteWrapper.GetFlag(flag1, 0);
            showExperienceLevelFloor = BooleanByteWrapper.GetFlag(flag1, 1);
            showExperienceNextLevelFloor = BooleanByteWrapper.GetFlag(flag1, 2);
            showExperienceFightDelta = BooleanByteWrapper.GetFlag(flag1, 3);
            showExperienceForGuild = BooleanByteWrapper.GetFlag(flag1, 4);
            showExperienceForMount = BooleanByteWrapper.GetFlag(flag1, 5);
            isIncarnationExperience = BooleanByteWrapper.GetFlag(flag1, 6);
            experience = reader.ReadVarUhLong();
            if ((experience < 0) || (experience > 9.007199254740992E15))
                throw new Exception("Forbidden value on experience = " + experience + ", it doesn't respect the following condition : (experience < 0) || (experience > 9.007199254740992E15)");
            experienceLevelFloor = reader.ReadVarUhLong();
            if ((experienceLevelFloor < 0) || (experienceLevelFloor > 9.007199254740992E15))
                throw new Exception("Forbidden value on experienceLevelFloor = " + experienceLevelFloor + ", it doesn't respect the following condition : (experienceLevelFloor < 0) || (experienceLevelFloor > 9.007199254740992E15)");
            experienceNextLevelFloor = reader.ReadDouble();
            if ((experienceNextLevelFloor < 0) || (experienceNextLevelFloor > 9.007199254740992E15))
                throw new Exception("Forbidden value on experienceNextLevelFloor = " + experienceNextLevelFloor + ", it doesn't respect the following condition : (experienceNextLevelFloor < 0) || (experienceNextLevelFloor > 9.007199254740992E15)");
            experienceFightDelta = reader.ReadVarInt();
            experienceForGuild = reader.ReadVarUhInt();
            if (experienceForGuild < 0)
                throw new Exception("Forbidden value on experienceForGuild = " + experienceForGuild + ", it doesn't respect the following condition : experienceForGuild < 0");
            experienceForMount = reader.ReadVarUhInt();
            if (experienceForMount < 0)
                throw new Exception("Forbidden value on experienceForMount = " + experienceForMount + ", it doesn't respect the following condition : experienceForMount < 0");
            rerollExperienceMul = reader.ReadSByte();
            if (rerollExperienceMul < 0)
                throw new Exception("Forbidden value on rerollExperienceMul = " + rerollExperienceMul + ", it doesn't respect the following condition : rerollExperienceMul < 0");
            

}


}


}