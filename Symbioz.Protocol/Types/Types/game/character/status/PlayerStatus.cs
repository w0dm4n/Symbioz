


















// Generated on 06/04/2015 18:45:23
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Types
{

public class PlayerStatus
{

public const short Id = 415;
public virtual short TypeId
{
    get { return Id; }
}

public sbyte statusId;
        

public PlayerStatus()
{
}

public PlayerStatus(sbyte statusId)
        {
            this.statusId = statusId;
        }
        

public virtual void Serialize(ICustomDataOutput writer)
{

writer.WriteSByte(statusId);
            

}

public virtual void Deserialize(ICustomDataInput reader)
{

statusId = reader.ReadSByte();
            if (statusId < 0)
                throw new Exception("Forbidden value on statusId = " + statusId + ", it doesn't respect the following condition : statusId < 0");
            

}


}


}