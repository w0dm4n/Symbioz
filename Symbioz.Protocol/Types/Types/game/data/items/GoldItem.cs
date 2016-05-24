


















// Generated on 06/04/2015 18:45:32
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Types
{

public class GoldItem : Item
{

public const short Id = 123;
public override short TypeId
{
    get { return Id; }
}

public uint sum;
        

public GoldItem()
{
}

public GoldItem(uint sum)
        {
            this.sum = sum;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteVarUhInt(sum);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            sum = reader.ReadVarUhInt();
            if (sum < 0)
                throw new Exception("Forbidden value on sum = " + sum + ", it doesn't respect the following condition : sum < 0");
            

}


}


}