


















// Generated on 06/04/2015 18:44:22
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class GameFightTurnListMessage : Message
{

public const ushort Id = 713;
public override ushort MessageId
{
    get { return Id; }
}

public IEnumerable<int> ids;
        public IEnumerable<int> deadsIds;
        

public GameFightTurnListMessage()
{
}

public GameFightTurnListMessage(IEnumerable<int> ids, IEnumerable<int> deadsIds)
        {
            this.ids = ids;
            this.deadsIds = deadsIds;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteUShort((ushort)ids.Count());
            foreach (var entry in ids)
            {
                 writer.WriteInt(entry);
            }
            writer.WriteUShort((ushort)deadsIds.Count());
            foreach (var entry in deadsIds)
            {
                 writer.WriteInt(entry);
            }
            

}

public override void Deserialize(ICustomDataInput reader)
{

var limit = reader.ReadUShort();
            ids = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                 (ids as int[])[i] = reader.ReadInt();
            }
            limit = reader.ReadUShort();
            deadsIds = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                 (deadsIds as int[])[i] = reader.ReadInt();
            }
            

}


}


}