


















// Generated on 06/04/2015 18:44:13
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class NumericWhoIsRequestMessage : Message
{

public const ushort Id = 6298;
public override ushort MessageId
{
    get { return Id; }
}

public uint playerId;
        

public NumericWhoIsRequestMessage()
{
}

public NumericWhoIsRequestMessage(uint playerId)
        {
            this.playerId = playerId;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteVarUhInt(playerId);
            

}

public override void Deserialize(ICustomDataInput reader)
{

playerId = reader.ReadVarUhInt();
            if (playerId < 0)
                throw new Exception("Forbidden value on playerId = " + playerId + ", it doesn't respect the following condition : playerId < 0");
            

}


}


}