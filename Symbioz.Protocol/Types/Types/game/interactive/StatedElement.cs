


















// Generated on 06/04/2015 18:45:35
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Types
{

public class StatedElement
{

public const short Id = 108;
public virtual short TypeId
{
    get { return Id; }
}

public int elementId;
        public ushort elementCellId;
        public uint elementState;
        

public StatedElement()
{
}

public StatedElement(int elementId, ushort elementCellId, uint elementState)
        {
            this.elementId = elementId;
            this.elementCellId = elementCellId;
            this.elementState = elementState;
        }
        

public virtual void Serialize(ICustomDataOutput writer)
{

writer.WriteInt(elementId);
            writer.WriteVarUhShort(elementCellId);
            writer.WriteVarUhInt(elementState);
            

}

public virtual void Deserialize(ICustomDataInput reader)
{

elementId = reader.ReadInt();
            if (elementId < 0)
                throw new Exception("Forbidden value on elementId = " + elementId + ", it doesn't respect the following condition : elementId < 0");
            elementCellId = reader.ReadVarUhShort();
            if ((elementCellId < 0) || (elementCellId > 559))
                throw new Exception("Forbidden value on elementCellId = " + elementCellId + ", it doesn't respect the following condition : (elementCellId < 0) || (elementCellId > 559)");
            elementState = reader.ReadVarUhInt();
            if (elementState < 0)
                throw new Exception("Forbidden value on elementState = " + elementState + ", it doesn't respect the following condition : elementState < 0");
            

}


}


}