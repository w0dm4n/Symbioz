


















// Generated on 06/04/2015 18:45:36
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Types
{

public class PaddockInformations
{

public const short Id = 132;
public virtual short TypeId
{
    get { return Id; }
}

public ushort maxOutdoorMount;
        public ushort maxItems;
        

public PaddockInformations()
{
}

public PaddockInformations(ushort maxOutdoorMount, ushort maxItems)
        {
            this.maxOutdoorMount = maxOutdoorMount;
            this.maxItems = maxItems;
        }
        

public virtual void Serialize(ICustomDataOutput writer)
{

writer.WriteVarUhShort(maxOutdoorMount);
            writer.WriteVarUhShort(maxItems);
            

}

public virtual void Deserialize(ICustomDataInput reader)
{

maxOutdoorMount = reader.ReadVarUhShort();
            if (maxOutdoorMount < 0)
                throw new Exception("Forbidden value on maxOutdoorMount = " + maxOutdoorMount + ", it doesn't respect the following condition : maxOutdoorMount < 0");
            maxItems = reader.ReadVarUhShort();
            if (maxItems < 0)
                throw new Exception("Forbidden value on maxItems = " + maxItems + ", it doesn't respect the following condition : maxItems < 0");
            

}


}


}