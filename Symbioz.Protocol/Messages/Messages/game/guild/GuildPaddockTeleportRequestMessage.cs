


















// Generated on 06/04/2015 18:44:46
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class GuildPaddockTeleportRequestMessage : Message
{

public const ushort Id = 5957;
public override ushort MessageId
{
    get { return Id; }
}

public int paddockId;
        

public GuildPaddockTeleportRequestMessage()
{
}

public GuildPaddockTeleportRequestMessage(int paddockId)
        {
            this.paddockId = paddockId;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteInt(paddockId);
            

}

public override void Deserialize(ICustomDataInput reader)
{

paddockId = reader.ReadInt();
            

}


}


}