


















// Generated on 06/04/2015 18:44:46
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class GuildMemberLeavingMessage : Message
{

public const ushort Id = 5923;
public override ushort MessageId
{
    get { return Id; }
}

public bool kicked;
        public int memberId;
        

public GuildMemberLeavingMessage()
{
}

public GuildMemberLeavingMessage(bool kicked, int memberId)
        {
            this.kicked = kicked;
            this.memberId = memberId;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteBoolean(kicked);
            writer.WriteInt(memberId);
            

}

public override void Deserialize(ICustomDataInput reader)
{

kicked = reader.ReadBoolean();
            memberId = reader.ReadInt();
            

}


}


}