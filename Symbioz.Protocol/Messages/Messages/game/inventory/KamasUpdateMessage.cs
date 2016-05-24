


















// Generated on 06/04/2015 18:44:50
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class KamasUpdateMessage : Message
{

public const ushort Id = 5537;
public override ushort MessageId
{
    get { return Id; }
}

public int kamasTotal;
        

public KamasUpdateMessage()
{
}

public KamasUpdateMessage(int kamasTotal)
        {
            this.kamasTotal = kamasTotal;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteVarInt(kamasTotal);
            

}

public override void Deserialize(ICustomDataInput reader)
{

kamasTotal = reader.ReadVarInt();
            

}


}


}