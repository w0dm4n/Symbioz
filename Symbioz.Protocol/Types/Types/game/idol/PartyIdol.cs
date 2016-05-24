


















// Generated on 06/04/2015 18:45:34
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Types
{

public class PartyIdol : Idol
{

public const short Id = 490;
public override short TypeId
{
    get { return Id; }
}

public IEnumerable<int> ownersIds;
        

public PartyIdol()
{
}

public PartyIdol(ushort id, ushort xpBonusPercent, ushort dropBonusPercent, IEnumerable<int> ownersIds)
         : base(id, xpBonusPercent, dropBonusPercent)
        {
            this.ownersIds = ownersIds;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteUShort((ushort)ownersIds.Count());
            foreach (var entry in ownersIds)
            {
                 writer.WriteInt(entry);
            }
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            var limit = reader.ReadUShort();
            ownersIds = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                 (ownersIds as int[])[i] = reader.ReadInt();
            }
            

}


}


}