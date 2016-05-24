


















// Generated on 06/04/2015 18:45:18
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Types
{

public class StatisticData
{

public const short Id = 484;
public virtual short TypeId
{
    get { return Id; }
}

public ushort actionId;
        

public StatisticData()
{
}

public StatisticData(ushort actionId)
        {
            this.actionId = actionId;
        }
        

public virtual void Serialize(ICustomDataOutput writer)
{

writer.WriteVarUhShort(actionId);
            

}

public virtual void Deserialize(ICustomDataInput reader)
{

actionId = reader.ReadVarUhShort();
            if (actionId < 0)
                throw new Exception("Forbidden value on actionId = " + actionId + ", it doesn't respect the following condition : actionId < 0");
            

}


}


}