


















// Generated on 06/04/2015 18:44:31
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class LockableStateUpdateHouseDoorMessage : LockableStateUpdateAbstractMessage
{

public const ushort Id = 5668;
public override ushort MessageId
{
    get { return Id; }
}

public uint houseId;
        

public LockableStateUpdateHouseDoorMessage()
{
}

public LockableStateUpdateHouseDoorMessage(bool locked, uint houseId)
         : base(locked)
        {
            this.houseId = houseId;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteVarUhInt(houseId);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            houseId = reader.ReadVarUhInt();
            if (houseId < 0)
                throw new Exception("Forbidden value on houseId = " + houseId + ", it doesn't respect the following condition : houseId < 0");
            

}


}


}