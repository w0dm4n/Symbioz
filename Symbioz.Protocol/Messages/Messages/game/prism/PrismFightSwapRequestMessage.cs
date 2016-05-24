


















// Generated on 06/04/2015 18:45:11
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class PrismFightSwapRequestMessage : Message
{

public const ushort Id = 5901;
public override ushort MessageId
{
    get { return Id; }
}

public ushort subAreaId;
        public uint targetId;
        

public PrismFightSwapRequestMessage()
{
}

public PrismFightSwapRequestMessage(ushort subAreaId, uint targetId)
        {
            this.subAreaId = subAreaId;
            this.targetId = targetId;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteVarUhShort(subAreaId);
            writer.WriteVarUhInt(targetId);
            

}

public override void Deserialize(ICustomDataInput reader)
{

subAreaId = reader.ReadVarUhShort();
            if (subAreaId < 0)
                throw new Exception("Forbidden value on subAreaId = " + subAreaId + ", it doesn't respect the following condition : subAreaId < 0");
            targetId = reader.ReadVarUhInt();
            if (targetId < 0)
                throw new Exception("Forbidden value on targetId = " + targetId + ", it doesn't respect the following condition : targetId < 0");
            

}


}


}