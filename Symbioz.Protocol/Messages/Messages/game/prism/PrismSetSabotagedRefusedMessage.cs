


















// Generated on 06/04/2015 18:45:12
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class PrismSetSabotagedRefusedMessage : Message
{

public const ushort Id = 6466;
public override ushort MessageId
{
    get { return Id; }
}

public ushort subAreaId;
        public sbyte reason;
        

public PrismSetSabotagedRefusedMessage()
{
}

public PrismSetSabotagedRefusedMessage(ushort subAreaId, sbyte reason)
        {
            this.subAreaId = subAreaId;
            this.reason = reason;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteVarUhShort(subAreaId);
            writer.WriteSByte(reason);
            

}

public override void Deserialize(ICustomDataInput reader)
{

subAreaId = reader.ReadVarUhShort();
            if (subAreaId < 0)
                throw new Exception("Forbidden value on subAreaId = " + subAreaId + ", it doesn't respect the following condition : subAreaId < 0");
            reason = reader.ReadSByte();
            

}


}


}