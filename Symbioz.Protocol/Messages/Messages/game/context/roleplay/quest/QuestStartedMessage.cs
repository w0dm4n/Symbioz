


















// Generated on 06/04/2015 18:44:39
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class QuestStartedMessage : Message
{

public const ushort Id = 6091;
public override ushort MessageId
{
    get { return Id; }
}

public ushort questId;
        

public QuestStartedMessage()
{
}

public QuestStartedMessage(ushort questId)
        {
            this.questId = questId;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteVarUhShort(questId);
            

}

public override void Deserialize(ICustomDataInput reader)
{

questId = reader.ReadVarUhShort();
            if (questId < 0)
                throw new Exception("Forbidden value on questId = " + questId + ", it doesn't respect the following condition : questId < 0");
            

}


}


}