


















// Generated on 06/04/2015 18:44:20
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class GameFightPlacementSwapPositionsAcceptMessage : Message
{

public const ushort Id = 6547;
public override ushort MessageId
{
    get { return Id; }
}

public int requestId;
        

public GameFightPlacementSwapPositionsAcceptMessage()
{
}

public GameFightPlacementSwapPositionsAcceptMessage(int requestId)
        {
            this.requestId = requestId;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteInt(requestId);
            

}

public override void Deserialize(ICustomDataInput reader)
{

requestId = reader.ReadInt();
            if (requestId < 0)
                throw new Exception("Forbidden value on requestId = " + requestId + ", it doesn't respect the following condition : requestId < 0");
            

}


}


}