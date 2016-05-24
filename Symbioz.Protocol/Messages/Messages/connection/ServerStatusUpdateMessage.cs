


















// Generated on 06/04/2015 18:44:03
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class ServerStatusUpdateMessage : Message
{

public const ushort Id = 50;
public override ushort MessageId
{
    get { return Id; }
}

public Types.GameServerInformations server;
        

public ServerStatusUpdateMessage()
{
}

public ServerStatusUpdateMessage(Types.GameServerInformations server)
        {
            this.server = server;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

server.Serialize(writer);
            

}

public override void Deserialize(ICustomDataInput reader)
{

server = new Types.GameServerInformations();
            server.Deserialize(reader);
            

}


}


}