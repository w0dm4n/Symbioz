


















// Generated on 06/04/2015 18:44:18
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class GameContextKickMessage : Message
{

public const ushort Id = 6081;
public override ushort MessageId
{
    get { return Id; }
}

public int targetId;
        

public GameContextKickMessage()
{
}

public GameContextKickMessage(int targetId)
        {
            this.targetId = targetId;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteInt(targetId);
            

}

public override void Deserialize(ICustomDataInput reader)
{

targetId = reader.ReadInt();
            

}


}


}