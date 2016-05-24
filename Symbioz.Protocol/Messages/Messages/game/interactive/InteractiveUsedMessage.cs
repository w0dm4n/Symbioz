


















// Generated on 06/04/2015 18:44:48
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class InteractiveUsedMessage : Message
{

public const ushort Id = 5745;
public override ushort MessageId
{
    get { return Id; }
}

public uint entityId;
        public uint elemId;
        public ushort skillId;
        public ushort duration;
        

public InteractiveUsedMessage()
{
}

public InteractiveUsedMessage(uint entityId, uint elemId, ushort skillId, ushort duration)
        {
            this.entityId = entityId;
            this.elemId = elemId;
            this.skillId = skillId;
            this.duration = duration;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteVarUhInt(entityId);
            writer.WriteVarUhInt(elemId);
            writer.WriteVarUhShort(skillId);
            writer.WriteVarUhShort(duration);
            

}

public override void Deserialize(ICustomDataInput reader)
{

entityId = reader.ReadVarUhInt();
            if (entityId < 0)
                throw new Exception("Forbidden value on entityId = " + entityId + ", it doesn't respect the following condition : entityId < 0");
            elemId = reader.ReadVarUhInt();
            if (elemId < 0)
                throw new Exception("Forbidden value on elemId = " + elemId + ", it doesn't respect the following condition : elemId < 0");
            skillId = reader.ReadVarUhShort();
            if (skillId < 0)
                throw new Exception("Forbidden value on skillId = " + skillId + ", it doesn't respect the following condition : skillId < 0");
            duration = reader.ReadVarUhShort();
            if (duration < 0)
                throw new Exception("Forbidden value on duration = " + duration + ", it doesn't respect the following condition : duration < 0");
            

}


}


}