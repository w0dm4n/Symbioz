


















// Generated on 06/04/2015 18:44:44
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class GuildInvitationAnswerMessage : Message
{

public const ushort Id = 5556;
public override ushort MessageId
{
    get { return Id; }
}

public bool accept;
        

public GuildInvitationAnswerMessage()
{
}

public GuildInvitationAnswerMessage(bool accept)
        {
            this.accept = accept;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteBoolean(accept);
            

}

public override void Deserialize(ICustomDataInput reader)
{

accept = reader.ReadBoolean();
            

}


}


}