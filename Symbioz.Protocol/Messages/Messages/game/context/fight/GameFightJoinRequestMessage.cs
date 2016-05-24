


















// Generated on 06/04/2015 18:44:19
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class GameFightJoinRequestMessage : Message
{

public const ushort Id = 701;
public override ushort MessageId
{
    get { return Id; }
}

public int fighterId;
        public int fightId;
        

public GameFightJoinRequestMessage()
{
}

public GameFightJoinRequestMessage(int fighterId, int fightId)
        {
            this.fighterId = fighterId;
            this.fightId = fightId;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteInt(fighterId);
            writer.WriteInt(fightId);
            

}

public override void Deserialize(ICustomDataInput reader)
{

fighterId = reader.ReadInt();
            fightId = reader.ReadInt();
            

}


}


}