


















// Generated on 06/04/2015 18:44:22
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class GameFightTurnReadyMessage : Message
{

public const ushort Id = 716;
public override ushort MessageId
{
    get { return Id; }
}

public bool isReady;
        

public GameFightTurnReadyMessage()
{
}

public GameFightTurnReadyMessage(bool isReady)
        {
            this.isReady = isReady;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteBoolean(isReady);
            

}

public override void Deserialize(ICustomDataInput reader)
{

isReady = reader.ReadBoolean();
            

}


}


}