


















// Generated on 06/04/2015 18:44:44
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class GuildInformationsMemberUpdateMessage : Message
{

public const ushort Id = 5597;
public override ushort MessageId
{
    get { return Id; }
}

public Types.GuildMember member;
        

public GuildInformationsMemberUpdateMessage()
{
}

public GuildInformationsMemberUpdateMessage(Types.GuildMember member)
        {
            this.member = member;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

member.Serialize(writer);
            

}

public override void Deserialize(ICustomDataInput reader)
{

member = new Types.GuildMember();
            member.Deserialize(reader);
            

}


}


}