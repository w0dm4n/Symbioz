


















// Generated on 06/04/2015 18:45:18
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Types
{

public class StatisticDataShort : StatisticData
{

public const short Id = 488;
public override short TypeId
{
    get { return Id; }
}

public short value;
        

public StatisticDataShort()
{
}

public StatisticDataShort(ushort actionId, short value)
         : base(actionId)
        {
            this.value = value;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteShort(value);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            value = reader.ReadShort();
            

}


}


}