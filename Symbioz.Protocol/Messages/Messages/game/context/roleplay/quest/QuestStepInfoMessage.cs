


















// Generated on 06/04/2015 18:44:40
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class QuestStepInfoMessage : Message
{

public const ushort Id = 5625;
public override ushort MessageId
{
    get { return Id; }
}

public Types.QuestActiveInformations infos;
        

public QuestStepInfoMessage()
{
}

public QuestStepInfoMessage(Types.QuestActiveInformations infos)
        {
            this.infos = infos;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteShort(infos.TypeId);
            infos.Serialize(writer);
            

}

public override void Deserialize(ICustomDataInput reader)
{

infos = Types.ProtocolTypeManager.GetInstance<Types.QuestActiveInformations>(reader.ReadShort());
            infos.Deserialize(reader);
            

}


}


}