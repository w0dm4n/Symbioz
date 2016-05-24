


















// Generated on 06/04/2015 18:44:07
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class GameActionFightInvisibleObstacleMessage : AbstractGameActionMessage
{

public const ushort Id = 5820;
public override ushort MessageId
{
    get { return Id; }
}

public uint sourceSpellId;
        

public GameActionFightInvisibleObstacleMessage()
{
}

public GameActionFightInvisibleObstacleMessage(ushort actionId, int sourceId, uint sourceSpellId)
         : base(actionId, sourceId)
        {
            this.sourceSpellId = sourceSpellId;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteVarUhInt(sourceSpellId);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            sourceSpellId = reader.ReadVarUhInt();
            if (sourceSpellId < 0)
                throw new Exception("Forbidden value on sourceSpellId = " + sourceSpellId + ", it doesn't respect the following condition : sourceSpellId < 0");
            

}


}


}