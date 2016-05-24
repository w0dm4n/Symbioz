


















// Generated on 06/04/2015 18:45:06
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class ObjectUseOnCellMessage : ObjectUseMessage
{

public const ushort Id = 3013;
public override ushort MessageId
{
    get { return Id; }
}

public ushort cells;
        

public ObjectUseOnCellMessage()
{
}

public ObjectUseOnCellMessage(uint objectUID, ushort cells)
         : base(objectUID)
        {
            this.cells = cells;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteVarUhShort(cells);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            cells = reader.ReadVarUhShort();
            if ((cells < 0) || (cells > 559))
                throw new Exception("Forbidden value on cells = " + cells + ", it doesn't respect the following condition : (cells < 0) || (cells > 559)");
            

}


}


}