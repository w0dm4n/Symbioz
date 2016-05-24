


















// Generated on 06/04/2015 18:45:23
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Types
{

public class CharacterRemodelingInformation : AbstractCharacterInformation
{

public const short Id = 479;
public override short TypeId
{
    get { return Id; }
}

public string name;
        public sbyte breed;
        public bool sex;
        public ushort cosmeticId;
        public IEnumerable<int> colors;
        

public CharacterRemodelingInformation()
{
}

public CharacterRemodelingInformation(uint id, string name, sbyte breed, bool sex, ushort cosmeticId, IEnumerable<int> colors)
         : base(id)
        {
            this.name = name;
            this.breed = breed;
            this.sex = sex;
            this.cosmeticId = cosmeticId;
            this.colors = colors;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteUTF(name);
            writer.WriteSByte(breed);
            writer.WriteBoolean(sex);
            writer.WriteVarUhShort(cosmeticId);
            writer.WriteUShort((ushort)colors.Count());
            foreach (var entry in colors)
            {
                 writer.WriteInt(entry);
            }
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            name = reader.ReadUTF();
            breed = reader.ReadSByte();
            sex = reader.ReadBoolean();
            cosmeticId = reader.ReadVarUhShort();
            if (cosmeticId < 0)
                throw new Exception("Forbidden value on cosmeticId = " + cosmeticId + ", it doesn't respect the following condition : cosmeticId < 0");
            var limit = reader.ReadUShort();
            colors = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                 (colors as int[])[i] = reader.ReadInt();
            }
            

}


}


}