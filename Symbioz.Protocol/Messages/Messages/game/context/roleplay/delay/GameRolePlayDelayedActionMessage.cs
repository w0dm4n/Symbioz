


















// Generated on 06/04/2015 18:44:26
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class GameRolePlayDelayedActionMessage : Message
{

public const ushort Id = 6153;
public override ushort MessageId
{
    get { return Id; }
}

public int delayedCharacterId;
        public sbyte delayTypeId;
        public double delayEndTime;
        

public GameRolePlayDelayedActionMessage()
{
}

public GameRolePlayDelayedActionMessage(int delayedCharacterId, sbyte delayTypeId, double delayEndTime)
        {
            this.delayedCharacterId = delayedCharacterId;
            this.delayTypeId = delayTypeId;
            this.delayEndTime = delayEndTime;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteInt(delayedCharacterId);
            writer.WriteSByte(delayTypeId);
            writer.WriteDouble(delayEndTime);
            

}

public override void Deserialize(ICustomDataInput reader)
{

delayedCharacterId = reader.ReadInt();
            delayTypeId = reader.ReadSByte();
            if (delayTypeId < 0)
                throw new Exception("Forbidden value on delayTypeId = " + delayTypeId + ", it doesn't respect the following condition : delayTypeId < 0");
            delayEndTime = reader.ReadDouble();
            if ((delayEndTime < 0) || (delayEndTime > 9.007199254740992E15))
                throw new Exception("Forbidden value on delayEndTime = " + delayEndTime + ", it doesn't respect the following condition : (delayEndTime < 0) || (delayEndTime > 9.007199254740992E15)");
            

}


}


}