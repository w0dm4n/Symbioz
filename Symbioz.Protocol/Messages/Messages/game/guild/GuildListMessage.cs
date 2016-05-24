


















// Generated on 06/04/2015 18:44:46
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class GuildListMessage : Message
{

public const ushort Id = 6413;
public override ushort MessageId
{
    get { return Id; }
}

public IEnumerable<Types.GuildInformations> guilds;
        

public GuildListMessage()
{
}

public GuildListMessage(IEnumerable<Types.GuildInformations> guilds)
        {
            this.guilds = guilds;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteUShort((ushort)guilds.Count());
            foreach (var entry in guilds)
            {
                 entry.Serialize(writer);
            }
            

}

public override void Deserialize(ICustomDataInput reader)
{

var limit = reader.ReadUShort();
            guilds = new Types.GuildInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 (guilds as Types.GuildInformations[])[i] = new Types.GuildInformations();
                 (guilds as Types.GuildInformations[])[i].Deserialize(reader);
            }
            

}


}


}