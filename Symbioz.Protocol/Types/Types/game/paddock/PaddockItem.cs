


















// Generated on 06/04/2015 18:45:36
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Types
{

public class PaddockItem : ObjectItemInRolePlay
{

public const short Id = 185;
public override short TypeId
{
    get { return Id; }
}

public Types.ItemDurability durability;
        

public PaddockItem()
{
}

public PaddockItem(ushort cellId, ushort objectGID, Types.ItemDurability durability)
         : base(cellId, objectGID)
        {
            this.durability = durability;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            durability.Serialize(writer);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            durability = new Types.ItemDurability();
            durability.Deserialize(reader);
            

}


}


}