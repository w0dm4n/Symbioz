


















// Generated on 06/04/2015 18:45:31
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Types
{

public class PortalInformation
{

public const short Id = 466;
public virtual short TypeId
{
    get { return Id; }
}

public ushort portalId;
        public short areaId;
        

public PortalInformation()
{
}

public PortalInformation(ushort portalId, short areaId)
        {
            this.portalId = portalId;
            this.areaId = areaId;
        }
        

public virtual void Serialize(ICustomDataOutput writer)
{

writer.WriteVarUhShort(portalId);
            writer.WriteShort(areaId);
            

}

public virtual void Deserialize(ICustomDataInput reader)
{

portalId = reader.ReadVarUhShort();
            if (portalId < 0)
                throw new Exception("Forbidden value on portalId = " + portalId + ", it doesn't respect the following condition : portalId < 0");
            areaId = reader.ReadShort();
            

}


}


}