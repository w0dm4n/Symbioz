


















// Generated on 06/04/2015 18:45:21
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Types
{

public class ServerSessionConstantInteger : ServerSessionConstant
{

public const short Id = 433;
public override short TypeId
{
    get { return Id; }
}

public int value;
        

public ServerSessionConstantInteger()
{
}

public ServerSessionConstantInteger(ushort id, int value)
         : base(id)
        {
            this.value = value;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteInt(value);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            value = reader.ReadInt();
            

}


}


}