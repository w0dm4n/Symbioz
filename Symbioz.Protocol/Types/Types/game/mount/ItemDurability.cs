


















// Generated on 06/04/2015 18:45:36
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Types
{

public class ItemDurability
{

public const short Id = 168;
public virtual short TypeId
{
    get { return Id; }
}

public short durability;
        public short durabilityMax;
        

public ItemDurability()
{
}

public ItemDurability(short durability, short durabilityMax)
        {
            this.durability = durability;
            this.durabilityMax = durabilityMax;
        }
        

public virtual void Serialize(ICustomDataOutput writer)
{

writer.WriteShort(durability);
            writer.WriteShort(durabilityMax);
            

}

public virtual void Deserialize(ICustomDataInput reader)
{

durability = reader.ReadShort();
            durabilityMax = reader.ReadShort();
            

}


}


}