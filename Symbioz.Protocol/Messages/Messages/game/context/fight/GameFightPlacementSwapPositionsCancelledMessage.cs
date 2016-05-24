


















// Generated on 06/04/2015 18:44:20
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class GameFightPlacementSwapPositionsCancelledMessage : Message
{

public const ushort Id = 6546;
public override ushort MessageId
{
    get { return Id; }
}

public int requestId;
        public uint cancellerId;
        

public GameFightPlacementSwapPositionsCancelledMessage()
{
}

public GameFightPlacementSwapPositionsCancelledMessage(int requestId, uint cancellerId)
        {
            this.requestId = requestId;
            this.cancellerId = cancellerId;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteInt(requestId);
            writer.WriteVarUhInt(cancellerId);
            

}

public override void Deserialize(ICustomDataInput reader)
{

requestId = reader.ReadInt();
            if (requestId < 0)
                throw new Exception("Forbidden value on requestId = " + requestId + ", it doesn't respect the following condition : requestId < 0");
            cancellerId = reader.ReadVarUhInt();
            if (cancellerId < 0)
                throw new Exception("Forbidden value on cancellerId = " + cancellerId + ", it doesn't respect the following condition : cancellerId < 0");
            

}


}


}