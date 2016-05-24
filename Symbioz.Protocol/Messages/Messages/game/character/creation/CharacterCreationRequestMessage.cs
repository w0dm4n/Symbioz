


















// Generated on 06/04/2015 18:44:14
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class CharacterCreationRequestMessage : Message
{

public const ushort Id = 160;
public override ushort MessageId
{
    get { return Id; }
}

public string name;
        public sbyte breed;
        public bool sex;
        public IEnumerable<int> colors;
        public ushort cosmeticId;
        

public CharacterCreationRequestMessage()
{
}

public CharacterCreationRequestMessage(string name, sbyte breed, bool sex, IEnumerable<int> colors, ushort cosmeticId)
        {
            this.name = name;
            this.breed = breed;
            this.sex = sex;
            this.colors = colors;
            this.cosmeticId = cosmeticId;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteUTF(name);
            writer.WriteSByte(breed);
            writer.WriteBoolean(sex);
            writer.WriteUShort((ushort)colors.Count());
            foreach (var entry in colors)
            {
                 writer.WriteInt(entry);
            }
            writer.WriteVarUhShort(cosmeticId);
            

}

public override void Deserialize(ICustomDataInput reader)
{

name = reader.ReadUTF();
            breed = reader.ReadSByte();
            if ((breed < (byte)Enums.PlayableBreedEnum.Feca) || (breed > (byte)Enums.PlayableBreedEnum.Eliotrope))
                throw new Exception("Forbidden value on breed = " + breed + ", it doesn't respect the following condition : (breed < (byte)Enums.PlayableBreedEnum.Feca) || (breed > (byte)Enums.PlayableBreedEnum.Eliotrope)");
            sex = reader.ReadBoolean();
            colors = new int[5];
            for (int i = 0; i < 5; i++)
            {
                 (colors as int[])[i] = reader.ReadInt();
            }
            cosmeticId = reader.ReadVarUhShort();
            if (cosmeticId < 0)
                throw new Exception("Forbidden value on cosmeticId = " + cosmeticId + ", it doesn't respect the following condition : cosmeticId < 0");
            

}


}


}