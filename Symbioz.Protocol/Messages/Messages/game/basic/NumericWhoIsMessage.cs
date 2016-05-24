


















// Generated on 06/04/2015 18:44:13
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class NumericWhoIsMessage : Message
{

public const ushort Id = 6297;
public override ushort MessageId
{
    get { return Id; }
}

public uint playerId;
        public int accountId;
        

public NumericWhoIsMessage()
{
}

public NumericWhoIsMessage(uint playerId, int accountId)
        {
            this.playerId = playerId;
            this.accountId = accountId;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteVarUhInt(playerId);
            writer.WriteInt(accountId);
            

}

public override void Deserialize(ICustomDataInput reader)
{

playerId = reader.ReadVarUhInt();
            if (playerId < 0)
                throw new Exception("Forbidden value on playerId = " + playerId + ", it doesn't respect the following condition : playerId < 0");
            accountId = reader.ReadInt();
            if (accountId < 0)
                throw new Exception("Forbidden value on accountId = " + accountId + ", it doesn't respect the following condition : accountId < 0");
            

}


}


}