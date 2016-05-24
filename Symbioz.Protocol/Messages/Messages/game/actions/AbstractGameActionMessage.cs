


















// Generated on 06/04/2015 18:44:05
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class AbstractGameActionMessage : Message
{

public const ushort Id = 1000;
public override ushort MessageId
{
    get { return Id; }
}

public ushort actionId;
        public int sourceId;
        

public AbstractGameActionMessage()
{
}

public AbstractGameActionMessage(ushort actionId, int sourceId)
        {
            this.actionId = actionId;
            this.sourceId = sourceId;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteVarUhShort(actionId);
            writer.WriteInt(sourceId);
            

}

public override void Deserialize(ICustomDataInput reader)
{

actionId = reader.ReadVarUhShort();
            if (actionId < 0)
                throw new Exception("Forbidden value on actionId = " + actionId + ", it doesn't respect the following condition : actionId < 0");
            sourceId = reader.ReadInt();
            

}


}


}