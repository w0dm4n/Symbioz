


















// Generated on 06/04/2015 18:45:25
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Types
{

public class GameFightFighterLightInformations
{

public const short Id = 413;
public virtual short TypeId
{
    get { return Id; }
}

public bool sex;
        public bool alive;
        public int id;
        public sbyte wave;
        public ushort level;
        public sbyte breed;
        

public GameFightFighterLightInformations()
{
}

public GameFightFighterLightInformations(bool sex, bool alive, int id, sbyte wave, ushort level, sbyte breed)
        {
            this.sex = sex;
            this.alive = alive;
            this.id = id;
            this.wave = wave;
            this.level = level;
            this.breed = breed;
        }
        

public virtual void Serialize(ICustomDataOutput writer)
{

byte flag1 = 0;
            flag1 = BooleanByteWrapper.SetFlag(flag1, 0, sex);
            flag1 = BooleanByteWrapper.SetFlag(flag1, 1, alive);
            writer.WriteByte(flag1);
            writer.WriteInt(id);
            writer.WriteSByte(wave);
            writer.WriteVarUhShort(level);
            writer.WriteSByte(breed);
            

}

public virtual void Deserialize(ICustomDataInput reader)
{

byte flag1 = reader.ReadByte();
            sex = BooleanByteWrapper.GetFlag(flag1, 0);
            alive = BooleanByteWrapper.GetFlag(flag1, 1);
            id = reader.ReadInt();
            wave = reader.ReadSByte();
            if (wave < 0)
                throw new Exception("Forbidden value on wave = " + wave + ", it doesn't respect the following condition : wave < 0");
            level = reader.ReadVarUhShort();
            if (level < 0)
                throw new Exception("Forbidden value on level = " + level + ", it doesn't respect the following condition : level < 0");
            breed = reader.ReadSByte();
            

}


}


}