


















// Generated on 06/04/2015 18:44:54
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class ExchangeOkMultiCraftMessage : Message
{

public const ushort Id = 5768;
public override ushort MessageId
{
    get { return Id; }
}

public uint initiatorId;
        public uint otherId;
        public sbyte role;
        

public ExchangeOkMultiCraftMessage()
{
}

public ExchangeOkMultiCraftMessage(uint initiatorId, uint otherId, sbyte role)
        {
            this.initiatorId = initiatorId;
            this.otherId = otherId;
            this.role = role;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteVarUhInt(initiatorId);
            writer.WriteVarUhInt(otherId);
            writer.WriteSByte(role);
            

}

public override void Deserialize(ICustomDataInput reader)
{

initiatorId = reader.ReadVarUhInt();
            if (initiatorId < 0)
                throw new Exception("Forbidden value on initiatorId = " + initiatorId + ", it doesn't respect the following condition : initiatorId < 0");
            otherId = reader.ReadVarUhInt();
            if (otherId < 0)
                throw new Exception("Forbidden value on otherId = " + otherId + ", it doesn't respect the following condition : otherId < 0");
            role = reader.ReadSByte();
            

}


}


}