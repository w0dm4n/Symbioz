


















// Generated on 06/04/2015 18:44:37
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class PartyNameSetRequestMessage : AbstractPartyMessage
{

public const ushort Id = 6503;
public override ushort MessageId
{
    get { return Id; }
}

public string partyName;
        

public PartyNameSetRequestMessage()
{
}

public PartyNameSetRequestMessage(uint partyId, string partyName)
         : base(partyId)
        {
            this.partyName = partyName;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteUTF(partyName);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            partyName = reader.ReadUTF();
            

}


}


}