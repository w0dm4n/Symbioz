


















// Generated on 06/04/2015 18:45:17
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class KrosmasterInventoryMessage : Message
{

public const ushort Id = 6350;
public override ushort MessageId
{
    get { return Id; }
}

public IEnumerable<Types.KrosmasterFigure> figures;
        

public KrosmasterInventoryMessage()
{
}

public KrosmasterInventoryMessage(IEnumerable<Types.KrosmasterFigure> figures)
        {
            this.figures = figures;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteUShort((ushort)figures.Count());
            foreach (var entry in figures)
            {
                 entry.Serialize(writer);
            }
            

}

public override void Deserialize(ICustomDataInput reader)
{

var limit = reader.ReadUShort();
            figures = new Types.KrosmasterFigure[limit];
            for (int i = 0; i < limit; i++)
            {
                 (figures as Types.KrosmasterFigure[])[i] = new Types.KrosmasterFigure();
                 (figures as Types.KrosmasterFigure[])[i].Deserialize(reader);
            }
            

}


}


}