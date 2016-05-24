


















// Generated on 06/04/2015 18:44:45
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class GuildInvitedMessage : Message
{

public const ushort Id = 5552;
public override ushort MessageId
{
    get { return Id; }
}

public uint recruterId;
        public string recruterName;
        public Types.BasicGuildInformations guildInfo;
        

public GuildInvitedMessage()
{
}

public GuildInvitedMessage(uint recruterId, string recruterName, Types.BasicGuildInformations guildInfo)
        {
            this.recruterId = recruterId;
            this.recruterName = recruterName;
            this.guildInfo = guildInfo;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteVarUhInt(recruterId);
            writer.WriteUTF(recruterName);
            guildInfo.Serialize(writer);
            

}

public override void Deserialize(ICustomDataInput reader)
{

recruterId = reader.ReadVarUhInt();
            if (recruterId < 0)
                throw new Exception("Forbidden value on recruterId = " + recruterId + ", it doesn't respect the following condition : recruterId < 0");
            recruterName = reader.ReadUTF();
            guildInfo = new Types.BasicGuildInformations();
            guildInfo.Deserialize(reader);
            

}


}


}