


















// Generated on 06/04/2015 18:44:44
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class GuildInformationsMembersMessage : Message
{

public const ushort Id = 5558;
public override ushort MessageId
{
    get { return Id; }
}

public IEnumerable<Types.GuildMember> members;
        

public GuildInformationsMembersMessage()
{
}

public GuildInformationsMembersMessage(IEnumerable<Types.GuildMember> members)
        {
            this.members = members;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteUShort((ushort)members.Count());
            foreach (var entry in members)
            {
                 entry.Serialize(writer);
            }
            

}

public override void Deserialize(ICustomDataInput reader)
{

var limit = reader.ReadUShort();
            members = new Types.GuildMember[limit];
            for (int i = 0; i < limit; i++)
            {
                 (members as Types.GuildMember[])[i] = new Types.GuildMember();
                 (members as Types.GuildMember[])[i].Deserialize(reader);
            }
            

}


}


}