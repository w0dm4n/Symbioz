


















// Generated on 06/04/2015 18:44:17
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class GameCautiousMapMovementMessage : GameMapMovementMessage
{

public const ushort Id = 6497;
public override ushort MessageId
{
    get { return Id; }
}



public GameCautiousMapMovementMessage()
{
}

public GameCautiousMapMovementMessage(IEnumerable<short> keyMovements, int actorId)
         : base(keyMovements, actorId)
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