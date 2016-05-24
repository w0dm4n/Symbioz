


















// Generated on 06/04/2015 18:44:27
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class GameRolePlayAggressionMessage : Message
{

public const ushort Id = 6073;
public override ushort MessageId
{
    get { return Id; }
}

public uint attackerId;
        public uint defenderId;
        

public GameRolePlayAggressionMessage()
{
}

public GameRolePlayAggressionMessage(uint attackerId, uint defenderId)
        {
            this.attackerId = attackerId;
            this.defenderId = defenderId;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteVarUhInt(attackerId);
            writer.WriteVarUhInt(defenderId);
            

}

public override void Deserialize(ICustomDataInput reader)
{

attackerId = reader.ReadVarUhInt();
            if (attackerId < 0)
                throw new Exception("Forbidden value on attackerId = " + attackerId + ", it doesn't respect the following condition : attackerId < 0");
            defenderId = reader.ReadVarUhInt();
            if (defenderId < 0)
                throw new Exception("Forbidden value on defenderId = " + defenderId + ", it doesn't respect the following condition : defenderId < 0");
            

}


}


}