


















// Generated on 06/04/2015 18:44:46
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class GuildFightJoinRequestMessage : Message
{

public const ushort Id = 5717;
public override ushort MessageId
{
    get { return Id; }
}

public int taxCollectorId;
        

public GuildFightJoinRequestMessage()
{
}

public GuildFightJoinRequestMessage(int taxCollectorId)
        {
            this.taxCollectorId = taxCollectorId;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteInt(taxCollectorId);
            

}

public override void Deserialize(ICustomDataInput reader)
{

taxCollectorId = reader.ReadInt();
            

}


}


}