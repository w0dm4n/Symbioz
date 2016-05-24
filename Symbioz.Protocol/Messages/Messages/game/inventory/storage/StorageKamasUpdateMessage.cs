


















// Generated on 06/04/2015 18:45:08
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class StorageKamasUpdateMessage : Message
{

public const ushort Id = 5645;
public override ushort MessageId
{
    get { return Id; }
}

public int kamasTotal;
        

public StorageKamasUpdateMessage()
{
}

public StorageKamasUpdateMessage(int kamasTotal)
        {
            this.kamasTotal = kamasTotal;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteInt(kamasTotal);
            

}

public override void Deserialize(ICustomDataInput reader)
{

kamasTotal = reader.ReadInt();
            

}


}


}