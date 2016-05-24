


















// Generated on 06/04/2015 18:44:26
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class GameRolePlayDelayedObjectUseMessage : GameRolePlayDelayedActionMessage
{

public const ushort Id = 6425;
public override ushort MessageId
{
    get { return Id; }
}

public ushort objectGID;
        

public GameRolePlayDelayedObjectUseMessage()
{
}

public GameRolePlayDelayedObjectUseMessage(int delayedCharacterId, sbyte delayTypeId, double delayEndTime, ushort objectGID)
         : base(delayedCharacterId, delayTypeId, delayEndTime)
        {
            this.objectGID = objectGID;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteVarUhShort(objectGID);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            objectGID = reader.ReadVarUhShort();
            if (objectGID < 0)
                throw new Exception("Forbidden value on objectGID = " + objectGID + ", it doesn't respect the following condition : objectGID < 0");
            

}


}


}