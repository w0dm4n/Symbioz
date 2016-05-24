


















// Generated on 06/04/2015 18:44:27
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class EmotePlayMassiveMessage : EmotePlayAbstractMessage
{

public const ushort Id = 5691;
public override ushort MessageId
{
    get { return Id; }
}

public IEnumerable<int> actorIds;
        

public EmotePlayMassiveMessage()
{
}

public EmotePlayMassiveMessage(byte emoteId, double emoteStartTime, IEnumerable<int> actorIds)
         : base(emoteId, emoteStartTime)
        {
            this.actorIds = actorIds;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteUShort((ushort)actorIds.Count());
            foreach (var entry in actorIds)
            {
                 writer.WriteInt(entry);
            }
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            var limit = reader.ReadUShort();
            actorIds = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                 (actorIds as int[])[i] = reader.ReadInt();
            }
            

}


}


}