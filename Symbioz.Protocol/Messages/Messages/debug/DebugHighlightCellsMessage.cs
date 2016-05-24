


















// Generated on 06/04/2015 18:44:04
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class DebugHighlightCellsMessage : Message
{

public const ushort Id = 2001;
public override ushort MessageId
{
    get { return Id; }
}

public int color;
        public IEnumerable<ushort> cells;
        

public DebugHighlightCellsMessage()
{
}

public DebugHighlightCellsMessage(int color, IEnumerable<ushort> cells)
        {
            this.color = color;
            this.cells = cells;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteInt(color);
            writer.WriteUShort((ushort)cells.Count());
            foreach (var entry in cells)
            {
                 writer.WriteVarUhShort(entry);
            }
            

}

public override void Deserialize(ICustomDataInput reader)
{

color = reader.ReadInt();
            var limit = reader.ReadUShort();
            cells = new ushort[limit];
            for (int i = 0; i < limit; i++)
            {
                 (cells as ushort[])[i] = reader.ReadVarUhShort();
            }
            

}


}


}