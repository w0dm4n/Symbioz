


















// Generated on 06/04/2015 18:44:27
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class GameRolePlayAttackMonsterRequestMessage : Message
{

public const ushort Id = 6191;
public override ushort MessageId
{
    get { return Id; }
}

public int monsterGroupId;
        

public GameRolePlayAttackMonsterRequestMessage()
{
}

public GameRolePlayAttackMonsterRequestMessage(int monsterGroupId)
        {
            this.monsterGroupId = monsterGroupId;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteInt(monsterGroupId);
            

}

public override void Deserialize(ICustomDataInput reader)
{

monsterGroupId = reader.ReadInt();
            

}


}


}