


















// Generated on 06/04/2015 18:44:22
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class GameDataPaddockObjectAddMessage : Message
{

public const ushort Id = 5990;
public override ushort MessageId
{
    get { return Id; }
}

public Types.PaddockItem paddockItemDescription;
        

public GameDataPaddockObjectAddMessage()
{
}

public GameDataPaddockObjectAddMessage(Types.PaddockItem paddockItemDescription)
        {
            this.paddockItemDescription = paddockItemDescription;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

paddockItemDescription.Serialize(writer);
            

}

public override void Deserialize(ICustomDataInput reader)
{

paddockItemDescription = new Types.PaddockItem();
            paddockItemDescription.Deserialize(reader);
            

}


}


}