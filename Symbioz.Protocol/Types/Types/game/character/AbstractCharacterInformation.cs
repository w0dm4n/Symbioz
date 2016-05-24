


















// Generated on 06/04/2015 18:45:21
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Types
{

public class AbstractCharacterInformation
{

public const short Id = 400;
public virtual short TypeId
{
    get { return Id; }
}

public uint id;
        

public AbstractCharacterInformation()
{
}

public AbstractCharacterInformation(uint id)
        {
            this.id = id;
        }
        

public virtual void Serialize(ICustomDataOutput writer)
{

writer.WriteVarUhInt(id);
            

}

public virtual void Deserialize(ICustomDataInput reader)
{

id = reader.ReadVarUhInt();
            if (id < 0)
                throw new Exception("Forbidden value on id = " + id + ", it doesn't respect the following condition : id < 0");
            

}


}


}