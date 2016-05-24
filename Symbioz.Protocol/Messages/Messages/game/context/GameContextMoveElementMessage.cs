


















// Generated on 06/04/2015 18:44:18
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class GameContextMoveElementMessage : Message
{

public const ushort Id = 253;
public override ushort MessageId
{
    get { return Id; }
}

public Types.EntityMovementInformations movement;
        

public GameContextMoveElementMessage()
{
}

public GameContextMoveElementMessage(Types.EntityMovementInformations movement)
        {
            this.movement = movement;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

movement.Serialize(writer);
            

}

public override void Deserialize(ICustomDataInput reader)
{

movement = new Types.EntityMovementInformations();
            movement.Deserialize(reader);
            

}


}


}