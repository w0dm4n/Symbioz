


















// Generated on 06/04/2015 18:45:23
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Types
{

public class AbstractCharacterToRefurbishInformation : AbstractCharacterInformation
{

public const short Id = 475;
public override short TypeId
{
    get { return Id; }
}

public IEnumerable<int> colors;
        public uint cosmeticId;
        

public AbstractCharacterToRefurbishInformation()
{
}

public AbstractCharacterToRefurbishInformation(uint id, IEnumerable<int> colors, uint cosmeticId)
         : base(id)
        {
            this.colors = colors;
            this.cosmeticId = cosmeticId;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteUShort((ushort)colors.Count());
            foreach (var entry in colors)
            {
                 writer.WriteInt(entry);
            }
            writer.WriteVarUhInt(cosmeticId);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            var limit = reader.ReadUShort();
            colors = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                 (colors as int[])[i] = reader.ReadInt();
            }
            cosmeticId = reader.ReadVarUhInt();
            if (cosmeticId < 0)
                throw new Exception("Forbidden value on cosmeticId = " + cosmeticId + ", it doesn't respect the following condition : cosmeticId < 0");
            

}


}


}