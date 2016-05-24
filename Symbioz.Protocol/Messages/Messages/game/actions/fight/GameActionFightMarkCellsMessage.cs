


















// Generated on 06/04/2015 18:44:07
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class GameActionFightMarkCellsMessage : AbstractGameActionMessage
{

public const ushort Id = 5540;
public override ushort MessageId
{
    get { return Id; }
}

public Types.GameActionMark mark;
        

public GameActionFightMarkCellsMessage()
{
}

public GameActionFightMarkCellsMessage(ushort actionId, int sourceId, Types.GameActionMark mark)
         : base(actionId, sourceId)
        {
            this.mark = mark;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            mark.Serialize(writer);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            mark = new Types.GameActionMark();
            mark.Deserialize(reader);
            

}


}


}