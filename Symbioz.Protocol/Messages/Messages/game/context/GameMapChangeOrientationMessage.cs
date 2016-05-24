


















// Generated on 06/04/2015 18:44:19
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class GameMapChangeOrientationMessage : Message
{

public const ushort Id = 946;
public override ushort MessageId
{
    get { return Id; }
}

public Types.ActorOrientation orientation;
        

public GameMapChangeOrientationMessage()
{
}

public GameMapChangeOrientationMessage(Types.ActorOrientation orientation)
        {
            this.orientation = orientation;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

orientation.Serialize(writer);
            

}

public override void Deserialize(ICustomDataInput reader)
{

orientation = new Types.ActorOrientation();
            orientation.Deserialize(reader);
            

}


}


}