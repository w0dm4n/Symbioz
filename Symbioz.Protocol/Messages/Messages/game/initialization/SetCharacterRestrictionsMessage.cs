


















// Generated on 06/04/2015 18:44:48
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class SetCharacterRestrictionsMessage : Message
{

public const ushort Id = 170;
public override ushort MessageId
{
    get { return Id; }
}

public int actorId;
        public Types.ActorRestrictionsInformations restrictions;
        

public SetCharacterRestrictionsMessage()
{
}

public SetCharacterRestrictionsMessage(int actorId, Types.ActorRestrictionsInformations restrictions)
        {
            this.actorId = actorId;
            this.restrictions = restrictions;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteInt(actorId);
            restrictions.Serialize(writer);
            

}

public override void Deserialize(ICustomDataInput reader)
{

actorId = reader.ReadInt();
            restrictions = new Types.ActorRestrictionsInformations();
            restrictions.Deserialize(reader);
            

}


}


}