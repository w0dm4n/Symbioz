


















// Generated on 06/04/2015 18:45:23
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Types
{

public class CharacterHardcoreOrEpicInformations : CharacterBaseInformations
{

public const short Id = 474;
public override short TypeId
{
    get { return Id; }
}

        public byte deathState;
        public ushort DeathCount;
        public byte DeathMaxLevel;
        

public CharacterHardcoreOrEpicInformations()
{
}

public CharacterHardcoreOrEpicInformations(uint id, byte level, string name, Types.EntityLook entityLook, sbyte breed, bool sex, byte deathState, ushort DeathCount, byte DeathMaxLevel)
         : base(id, level, name, entityLook, breed, sex)
        {
            this.deathState = deathState;
            this.DeathCount = DeathCount;
            this.DeathMaxLevel = DeathMaxLevel;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteByte(deathState);
            writer.WriteVarUhShort(DeathCount);
            writer.WriteByte(DeathMaxLevel);
            

}

public override void Deserialize(ICustomDataInput reader)
{

            base.Deserialize(reader);
            deathState = reader.ReadByte();
            if (deathState < 0)
                throw new Exception("Forbidden value on deathState = " + deathState + ", it doesn't respect the following condition : deathState < 0");
            DeathCount = reader.ReadVarUhShort();
            if (DeathCount < 0)
                throw new Exception("Forbidden value on DeathCount = " + DeathCount + ", it doesn't respect the following condition : DeathCount < 0");
            DeathMaxLevel = reader.ReadByte();
            if ((DeathMaxLevel < 1) || (DeathMaxLevel > 200))
                throw new Exception("Forbidden value on DeathMaxLevel = " + DeathMaxLevel + ", it doesn't respect the following condition : (DeathMaxLevel < 1) || (DeathMaxLevel > 200)");
            

}


}


}