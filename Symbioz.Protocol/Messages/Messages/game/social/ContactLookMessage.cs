


















// Generated on 06/04/2015 18:45:14
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class ContactLookMessage : Message
{

public const ushort Id = 5934;
public override ushort MessageId
{
    get { return Id; }
}

public uint requestId;
        public string playerName;
        public uint playerId;
        public Types.EntityLook look;
        

public ContactLookMessage()
{
}

public ContactLookMessage(uint requestId, string playerName, uint playerId, Types.EntityLook look)
        {
            this.requestId = requestId;
            this.playerName = playerName;
            this.playerId = playerId;
            this.look = look;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteVarUhInt(requestId);
            writer.WriteUTF(playerName);
            writer.WriteVarUhInt(playerId);
            look.Serialize(writer);
            

}

public override void Deserialize(ICustomDataInput reader)
{

requestId = reader.ReadVarUhInt();
            if (requestId < 0)
                throw new Exception("Forbidden value on requestId = " + requestId + ", it doesn't respect the following condition : requestId < 0");
            playerName = reader.ReadUTF();
            playerId = reader.ReadVarUhInt();
            if (playerId < 0)
                throw new Exception("Forbidden value on playerId = " + playerId + ", it doesn't respect the following condition : playerId < 0");
            look = new Types.EntityLook();
            look.Deserialize(reader);
            

}


}


}