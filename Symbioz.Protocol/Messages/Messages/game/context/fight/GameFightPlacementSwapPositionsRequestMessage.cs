


















// Generated on 06/04/2015 18:44:21
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class GameFightPlacementSwapPositionsRequestMessage : GameFightPlacementPositionRequestMessage
{

public const ushort Id = 6541;
public override ushort MessageId
{
    get { return Id; }
}

public int requestedId;
        

public GameFightPlacementSwapPositionsRequestMessage()
{
}

public GameFightPlacementSwapPositionsRequestMessage(ushort cellId, int requestedId)
         : base(cellId)
        {
            this.requestedId = requestedId;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteInt(requestedId);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            requestedId = reader.ReadInt();
            

}


}


}