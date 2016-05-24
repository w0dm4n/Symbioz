


















// Generated on 06/04/2015 18:45:18
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Types
{

public class StatisticDataString : StatisticData
{

public const short Id = 487;
public override short TypeId
{
    get { return Id; }
}

public string value;
        

public StatisticDataString()
{
}

public StatisticDataString(ushort actionId, string value)
         : base(actionId)
        {
            this.value = value;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteUTF(value);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            value = reader.ReadUTF();
            

}


}


}