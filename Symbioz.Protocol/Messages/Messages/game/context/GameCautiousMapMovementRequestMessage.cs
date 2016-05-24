


















// Generated on 06/04/2015 18:44:18
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class GameCautiousMapMovementRequestMessage : GameMapMovementRequestMessage
{

public const ushort Id = 6496;
public override ushort MessageId
{
    get { return Id; }
}



public GameCautiousMapMovementRequestMessage()
{
}

public GameCautiousMapMovementRequestMessage(IEnumerable<short> keyMovements, int mapId)
         : base(keyMovements, mapId)
        {
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            

}


}


}