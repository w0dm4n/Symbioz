


















// Generated on 06/04/2015 18:44:19
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class DungeonKeyRingMessage : Message
{

public const ushort Id = 6299;
public override ushort MessageId
{
    get { return Id; }
}

public IEnumerable<ushort> availables;
        public IEnumerable<ushort> unavailables;
        

public DungeonKeyRingMessage()
{
}

public DungeonKeyRingMessage(IEnumerable<ushort> availables, IEnumerable<ushort> unavailables)
        {
            this.availables = availables;
            this.unavailables = unavailables;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteUShort((ushort)availables.Count());
            foreach (var entry in availables)
            {
                 writer.WriteVarUhShort(entry);
            }
            writer.WriteUShort((ushort)unavailables.Count());
            foreach (var entry in unavailables)
            {
                 writer.WriteVarUhShort(entry);
            }
            

}

public override void Deserialize(ICustomDataInput reader)
{

var limit = reader.ReadUShort();
            availables = new ushort[limit];
            for (int i = 0; i < limit; i++)
            {
                 (availables as ushort[])[i] = reader.ReadVarUhShort();
            }
            limit = reader.ReadUShort();
            unavailables = new ushort[limit];
            for (int i = 0; i < limit; i++)
            {
                 (unavailables as ushort[])[i] = reader.ReadVarUhShort();
            }
            

}


}


}