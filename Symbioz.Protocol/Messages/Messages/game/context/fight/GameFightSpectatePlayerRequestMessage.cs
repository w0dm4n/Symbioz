


















// Generated on 06/04/2015 18:44:21
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class GameFightSpectatePlayerRequestMessage : Message
{

public const ushort Id = 6474;
public override ushort MessageId
{
    get { return Id; }
}

public int playerId;
        

public GameFightSpectatePlayerRequestMessage()
{
}

public GameFightSpectatePlayerRequestMessage(int playerId)
        {
            this.playerId = playerId;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteInt(playerId);
            

}

public override void Deserialize(ICustomDataInput reader)
{

playerId = reader.ReadInt();
            

}


}


}