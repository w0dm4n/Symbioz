


















// Generated on 06/04/2015 18:44:11
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class AllianceModificationEmblemValidMessage : Message
{

public const ushort Id = 6447;
public override ushort MessageId
{
    get { return Id; }
}

public Types.GuildEmblem Alliancemblem;
        

public AllianceModificationEmblemValidMessage()
{
}

public AllianceModificationEmblemValidMessage(Types.GuildEmblem Alliancemblem)
        {
            this.Alliancemblem = Alliancemblem;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

Alliancemblem.Serialize(writer);
            

}

public override void Deserialize(ICustomDataInput reader)
{

Alliancemblem = new Types.GuildEmblem();
            Alliancemblem.Deserialize(reader);
            

}


}


}