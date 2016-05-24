


















// Generated on 06/04/2015 18:45:14
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class ContactLookRequestByIdMessage : ContactLookRequestMessage
{

public const ushort Id = 5935;
public override ushort MessageId
{
    get { return Id; }
}

public uint playerId;
        

public ContactLookRequestByIdMessage()
{
}

public ContactLookRequestByIdMessage(byte requestId, sbyte contactType, uint playerId)
         : base(requestId, contactType)
        {
            this.playerId = playerId;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteVarUhInt(playerId);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            playerId = reader.ReadVarUhInt();
            if (playerId < 0)
                throw new Exception("Forbidden value on playerId = " + playerId + ", it doesn't respect the following condition : playerId < 0");
            

}


}


}