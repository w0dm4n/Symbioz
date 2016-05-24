


















// Generated on 06/04/2015 18:44:46
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class GuildMemberOnlineStatusMessage : Message
{

public const ushort Id = 6061;
public override ushort MessageId
{
    get { return Id; }
}

public uint memberId;
        public bool online;
        

public GuildMemberOnlineStatusMessage()
{
}

public GuildMemberOnlineStatusMessage(uint memberId, bool online)
        {
            this.memberId = memberId;
            this.online = online;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteVarUhInt(memberId);
            writer.WriteBoolean(online);
            

}

public override void Deserialize(ICustomDataInput reader)
{

memberId = reader.ReadVarUhInt();
            if (memberId < 0)
                throw new Exception("Forbidden value on memberId = " + memberId + ", it doesn't respect the following condition : memberId < 0");
            online = reader.ReadBoolean();
            

}


}


}