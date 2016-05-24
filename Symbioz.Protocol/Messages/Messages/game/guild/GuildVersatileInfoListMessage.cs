


















// Generated on 06/04/2015 18:44:46
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class GuildVersatileInfoListMessage : Message
{

public const ushort Id = 6435;
public override ushort MessageId
{
    get { return Id; }
}

public IEnumerable<Types.GuildVersatileInformations> guilds;
        

public GuildVersatileInfoListMessage()
{
}

public GuildVersatileInfoListMessage(IEnumerable<Types.GuildVersatileInformations> guilds)
        {
            this.guilds = guilds;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteUShort((ushort)guilds.Count());
            foreach (var entry in guilds)
            {
                 writer.WriteShort(entry.TypeId);
                 entry.Serialize(writer);
            }
            

}

public override void Deserialize(ICustomDataInput reader)
{

var limit = reader.ReadUShort();
            guilds = new Types.GuildVersatileInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 (guilds as Types.GuildVersatileInformations[])[i] = Types.ProtocolTypeManager.GetInstance<Types.GuildVersatileInformations>(reader.ReadShort());
                 (guilds as Types.GuildVersatileInformations[])[i].Deserialize(reader);
            }
            

}


}


}