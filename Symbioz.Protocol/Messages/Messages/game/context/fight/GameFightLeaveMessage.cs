


















// Generated on 06/04/2015 18:44:19
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class GameFightLeaveMessage : Message
{

public const ushort Id = 721;
public override ushort MessageId
{
    get { return Id; }
}

public int charId;
        

public GameFightLeaveMessage()
{
}

public GameFightLeaveMessage(int charId)
        {
            this.charId = charId;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteInt(charId);
            

}

public override void Deserialize(ICustomDataInput reader)
{

charId = reader.ReadInt();
            

}


}


}