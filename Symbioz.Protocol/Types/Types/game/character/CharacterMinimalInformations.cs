


















// Generated on 06/04/2015 18:45:22
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Types
{

public class CharacterMinimalInformations : AbstractCharacterInformation
{

public const short Id = 110;
public override short TypeId
{
    get { return Id; }
}

public byte level;
        public string name;
        

public CharacterMinimalInformations()
{
}

public CharacterMinimalInformations(uint id, byte level, string name)
         : base(id)
        {
            this.level = level;
            this.name = name;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteByte(level);
            writer.WriteUTF(name);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            level = reader.ReadByte();
            if ((level < 1) || (level > 200))
                throw new Exception("Forbidden value on level = " + level + ", it doesn't respect the following condition : (level < 1) || (level > 200)");
            name = reader.ReadUTF();
            

}


}


}