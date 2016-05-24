


















// Generated on 06/04/2015 18:44:28
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class GameRolePlayRemoveChallengeMessage : Message
{

public const ushort Id = 300;
public override ushort MessageId
{
    get { return Id; }
}

public int fightId;
        

public GameRolePlayRemoveChallengeMessage()
{
}

public GameRolePlayRemoveChallengeMessage(int fightId)
        {
            this.fightId = fightId;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteInt(fightId);
            

}

public override void Deserialize(ICustomDataInput reader)
{

fightId = reader.ReadInt();
            

}


}


}