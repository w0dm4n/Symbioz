


















// Generated on 06/04/2015 18:44:36
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class PartyInvitationRequestMessage : Message
{

public const ushort Id = 5585;
public override ushort MessageId
{
    get { return Id; }
}

public string name;
        

public PartyInvitationRequestMessage()
{
}

public PartyInvitationRequestMessage(string name)
        {
            this.name = name;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteUTF(name);
            

}

public override void Deserialize(ICustomDataInput reader)
{

name = reader.ReadUTF();
            

}


}


}