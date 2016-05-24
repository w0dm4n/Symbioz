


















// Generated on 06/04/2015 18:44:09
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class GameActionFightUnmarkCellsMessage : AbstractGameActionMessage
{

public const ushort Id = 5570;
public override ushort MessageId
{
    get { return Id; }
}

public short markId;
        

public GameActionFightUnmarkCellsMessage()
{
}

public GameActionFightUnmarkCellsMessage(ushort actionId, int sourceId, short markId)
         : base(actionId, sourceId)
        {
            this.markId = markId;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteShort(markId);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            markId = reader.ReadShort();
            

}


}


}