


















// Generated on 06/04/2015 18:44:43
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class ChallengeFightJoinRefusedMessage : Message
{

public const ushort Id = 5908;
public override ushort MessageId
{
    get { return Id; }
}

public uint playerId;
        public sbyte reason;
        

public ChallengeFightJoinRefusedMessage()
{
}

public ChallengeFightJoinRefusedMessage(uint playerId, sbyte reason)
        {
            this.playerId = playerId;
            this.reason = reason;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteVarUhInt(playerId);
            writer.WriteSByte(reason);
            

}

public override void Deserialize(ICustomDataInput reader)
{

playerId = reader.ReadVarUhInt();
            if (playerId < 0)
                throw new Exception("Forbidden value on playerId = " + playerId + ", it doesn't respect the following condition : playerId < 0");
            reason = reader.ReadSByte();
            

}


}


}