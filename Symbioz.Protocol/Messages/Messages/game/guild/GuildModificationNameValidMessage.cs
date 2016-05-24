


















// Generated on 06/04/2015 18:44:46
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class GuildModificationNameValidMessage : Message
{

public const ushort Id = 6327;
public override ushort MessageId
{
    get { return Id; }
}

public string guildName;
        

public GuildModificationNameValidMessage()
{
}

public GuildModificationNameValidMessage(string guildName)
        {
            this.guildName = guildName;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteUTF(guildName);
            

}

public override void Deserialize(ICustomDataInput reader)
{

guildName = reader.ReadUTF();
            

}


}


}