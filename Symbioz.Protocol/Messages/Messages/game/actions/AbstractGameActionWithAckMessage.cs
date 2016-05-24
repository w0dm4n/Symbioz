


















// Generated on 06/04/2015 18:44:05
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class AbstractGameActionWithAckMessage : AbstractGameActionMessage
{

public const ushort Id = 1001;
public override ushort MessageId
{
    get { return Id; }
}

public short waitAckId;
        

public AbstractGameActionWithAckMessage()
{
}

public AbstractGameActionWithAckMessage(ushort actionId, int sourceId, short waitAckId)
         : base(actionId, sourceId)
        {
            this.waitAckId = waitAckId;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteShort(waitAckId);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            waitAckId = reader.ReadShort();
            

}


}


}