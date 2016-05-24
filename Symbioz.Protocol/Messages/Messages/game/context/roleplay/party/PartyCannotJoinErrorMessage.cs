


















// Generated on 06/04/2015 18:44:34
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class PartyCannotJoinErrorMessage : AbstractPartyMessage
{

public const ushort Id = 5583;
public override ushort MessageId
{
    get { return Id; }
}

public sbyte reason;
        

public PartyCannotJoinErrorMessage()
{
}

public PartyCannotJoinErrorMessage(uint partyId, sbyte reason)
         : base(partyId)
        {
            this.reason = reason;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteSByte(reason);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            reason = reader.ReadSByte();
            if (reason < 0)
                throw new Exception("Forbidden value on reason = " + reason + ", it doesn't respect the following condition : reason < 0");
            

}


}


}