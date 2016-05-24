


















// Generated on 06/04/2015 18:44:49
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class TeleportBuddiesAnswerMessage : Message
{

public const ushort Id = 6294;
public override ushort MessageId
{
    get { return Id; }
}

public bool accept;
        

public TeleportBuddiesAnswerMessage()
{
}

public TeleportBuddiesAnswerMessage(bool accept)
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