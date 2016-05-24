


















// Generated on 06/04/2015 18:45:32
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Types
{

public class ObjectItemGenericQuantity : Item
{

public const short Id = 483;
public override short TypeId
{
    get { return Id; }
}

public ushort objectGID;
        public uint quantity;
        

public ObjectItemGenericQuantity()
{
}

public ObjectItemGenericQuantity(ushort objectGID, uint quantity)
        {
            this.objectGID = objectGID;
            this.quantity = quantity;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteVarUhShort(objectGID);
            writer.WriteVarUhInt(quantity);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            objectGID = reader.ReadVarUhShort();
            if (objectGID < 0)
                throw new Exception("Forbidden value on objectGID = " + objectGID + ", it doesn't respect the following condition : objectGID < 0");
            quantity = reader.ReadVarUhInt();
            if (quantity < 0)
                throw new Exception("Forbidden value on quantity = " + quantity + ", it doesn't respect the following condition : quantity < 0");
            

}


}


}