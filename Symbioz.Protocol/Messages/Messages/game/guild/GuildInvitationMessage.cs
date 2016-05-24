


















// Generated on 06/04/2015 18:44:44
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class GuildInvitationMessage : Message
{

public const ushort Id = 5551;
public override ushort MessageId
{
    get { return Id; }
}

public uint targetId;
        

public GuildInvitationMessage()
{
}

public GuildInvitationMessage(uint targetId)
        {
            this.targetId = targetId;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteVarUhInt(targetId);
            

}

public override void Deserialize(ICustomDataInput reader)
{

targetId = reader.ReadVarUhInt();
            if (targetId < 0)
                throw new Exception("Forbidden value on targetId = " + targetId + ", it doesn't respect the following condition : targetId < 0");
            

}


}


}