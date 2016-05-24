


















// Generated on 06/04/2015 18:45:18
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Types
{

public class StatisticDataByte : StatisticData
{

public const short Id = 486;
public override short TypeId
{
    get { return Id; }
}

public sbyte value;
        

public StatisticDataByte()
{
}

public StatisticDataByte(ushort actionId, sbyte value)
         : base(actionId)
        {
            this.value = value;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteSByte(value);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            value = reader.ReadSByte();
            

}


}


}