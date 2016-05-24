


















// Generated on 06/04/2015 18:44:40
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class SpellForgetUIMessage : Message
{

public const ushort Id = 5565;
public override ushort MessageId
{
    get { return Id; }
}

public bool open;
        

public SpellForgetUIMessage()
{
}

public SpellForgetUIMessage(bool open)
        {
            this.open = open;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteBoolean(open);
            

}

public override void Deserialize(ICustomDataInput reader)
{

open = reader.ReadBoolean();
            

}


}


}