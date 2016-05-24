


















// Generated on 06/04/2015 18:44:19
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class GameMapChangeOrientationRequestMessage : Message
{

public const ushort Id = 945;
public override ushort MessageId
{
    get { return Id; }
}

public sbyte direction;
        

public GameMapChangeOrientationRequestMessage()
{
}

public GameMapChangeOrientationRequestMessage(sbyte direction)
        {
            this.direction = direction;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteSByte(direction);
            

}

public override void Deserialize(ICustomDataInput reader)
{

direction = reader.ReadSByte();
            if (direction < 0)
                throw new Exception("Forbidden value on direction = " + direction + ", it doesn't respect the following condition : direction < 0");
            

}


}


}