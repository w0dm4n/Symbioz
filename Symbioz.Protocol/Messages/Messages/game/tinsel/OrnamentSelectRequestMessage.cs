


















// Generated on 06/04/2015 18:45:14
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class OrnamentSelectRequestMessage : Message
{

public const ushort Id = 6374;
public override ushort MessageId
{
    get { return Id; }
}

public ushort ornamentId;
        

public OrnamentSelectRequestMessage()
{
}

public OrnamentSelectRequestMessage(ushort ornamentId)
        {
            this.ornamentId = ornamentId;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteVarUhShort(ornamentId);
            

}

public override void Deserialize(ICustomDataInput reader)
{

ornamentId = reader.ReadVarUhShort();
            if (ornamentId < 0)
                throw new Exception("Forbidden value on ornamentId = " + ornamentId + ", it doesn't respect the following condition : ornamentId < 0");
            

}


}


}