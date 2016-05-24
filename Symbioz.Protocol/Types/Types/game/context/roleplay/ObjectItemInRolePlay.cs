


















// Generated on 06/04/2015 18:45:29
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Types
{

public class ObjectItemInRolePlay
{

public const short Id = 198;
public virtual short TypeId
{
    get { return Id; }
}

public ushort cellId;
        public ushort objectGID;
        

public ObjectItemInRolePlay()
{
}

public ObjectItemInRolePlay(ushort cellId, ushort objectGID)
        {
            this.cellId = cellId;
            this.objectGID = objectGID;
        }
        

public virtual void Serialize(ICustomDataOutput writer)
{

writer.WriteVarUhShort(cellId);
            writer.WriteVarUhShort(objectGID);
            

}

public virtual void Deserialize(ICustomDataInput reader)
{

cellId = reader.ReadVarUhShort();
            if ((cellId < 0) || (cellId > 559))
                throw new Exception("Forbidden value on cellId = " + cellId + ", it doesn't respect the following condition : (cellId < 0) || (cellId > 559)");
            objectGID = reader.ReadVarUhShort();
            if (objectGID < 0)
                throw new Exception("Forbidden value on objectGID = " + objectGID + ", it doesn't respect the following condition : objectGID < 0");
            

}


}


}