


















// Generated on 06/04/2015 18:44:09
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class GameActionFightTackledMessage : AbstractGameActionMessage
{

public const ushort Id = 1004;
public override ushort MessageId
{
    get { return Id; }
}

public IEnumerable<int> tacklersIds;
        

public GameActionFightTackledMessage()
{
}

public GameActionFightTackledMessage(ushort actionId, int sourceId, IEnumerable<int> tacklersIds)
         : base(actionId, sourceId)
        {
            this.tacklersIds = tacklersIds;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteUShort((ushort)tacklersIds.Count());
            foreach (var entry in tacklersIds)
            {
                 writer.WriteInt(entry);
            }
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            var limit = reader.ReadUShort();
            tacklersIds = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                 (tacklersIds as int[])[i] = reader.ReadInt();
            }
            

}


}


}