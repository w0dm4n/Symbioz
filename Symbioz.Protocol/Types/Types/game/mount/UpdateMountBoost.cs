


















// Generated on 06/04/2015 18:45:36
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Types
{

public class UpdateMountBoost
{

public const short Id = 356;
public virtual short TypeId
{
    get { return Id; }
}

public sbyte type;
        

public UpdateMountBoost()
{
}

public UpdateMountBoost(sbyte type)
        {
            this.type = type;
        }
        

public virtual void Serialize(ICustomDataOutput writer)
{

writer.WriteSByte(type);
            

}

public virtual void Deserialize(ICustomDataInput reader)
{

type = reader.ReadSByte();
            if (type < 0)
                throw new Exception("Forbidden value on type = " + type + ", it doesn't respect the following condition : type < 0");
            

}


}


}