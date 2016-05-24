


















// Generated on 06/04/2015 18:44:21
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class GameFightPlacementSwapPositionsOfferMessage : Message
{

public const ushort Id = 6542;
public override ushort MessageId
{
    get { return Id; }
}

public int requestId;
        public uint requesterId;
        public ushort requesterCellId;
        public uint requestedId;
        public ushort requestedCellId;
        

public GameFightPlacementSwapPositionsOfferMessage()
{
}

public GameFightPlacementSwapPositionsOfferMessage(int requestId, uint requesterId, ushort requesterCellId, uint requestedId, ushort requestedCellId)
        {
            this.requestId = requestId;
            this.requesterId = requesterId;
            this.requesterCellId = requesterCellId;
            this.requestedId = requestedId;
            this.requestedCellId = requestedCellId;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteInt(requestId);
            writer.WriteVarUhInt(requesterId);
            writer.WriteVarUhShort(requesterCellId);
            writer.WriteVarUhInt(requestedId);
            writer.WriteVarUhShort(requestedCellId);
            

}

public override void Deserialize(ICustomDataInput reader)
{

requestId = reader.ReadInt();
            if (requestId < 0)
                throw new Exception("Forbidden value on requestId = " + requestId + ", it doesn't respect the following condition : requestId < 0");
            requesterId = reader.ReadVarUhInt();
            if (requesterId < 0)
                throw new Exception("Forbidden value on requesterId = " + requesterId + ", it doesn't respect the following condition : requesterId < 0");
            requesterCellId = reader.ReadVarUhShort();
            if ((requesterCellId < 0) || (requesterCellId > 559))
                throw new Exception("Forbidden value on requesterCellId = " + requesterCellId + ", it doesn't respect the following condition : (requesterCellId < 0) || (requesterCellId > 559)");
            requestedId = reader.ReadVarUhInt();
            if (requestedId < 0)
                throw new Exception("Forbidden value on requestedId = " + requestedId + ", it doesn't respect the following condition : requestedId < 0");
            requestedCellId = reader.ReadVarUhShort();
            if ((requestedCellId < 0) || (requestedCellId > 559))
                throw new Exception("Forbidden value on requestedCellId = " + requestedCellId + ", it doesn't respect the following condition : (requestedCellId < 0) || (requestedCellId > 559)");
            

}


}


}