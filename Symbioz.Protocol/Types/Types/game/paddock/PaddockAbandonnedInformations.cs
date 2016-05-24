


















// Generated on 06/04/2015 18:45:36
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Types
{

public class PaddockAbandonnedInformations : PaddockBuyableInformations
{

public const short Id = 133;
public override short TypeId
{
    get { return Id; }
}

public int guildId;
        

public PaddockAbandonnedInformations()
{
}

public PaddockAbandonnedInformations(ushort maxOutdoorMount, ushort maxItems, uint price, bool locked, int guildId)
         : base(maxOutdoorMount, maxItems, price, locked)
        {
            this.guildId = guildId;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteInt(guildId);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            guildId = reader.ReadInt();
            

}


}


}