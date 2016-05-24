


















// Generated on 06/04/2015 18:45:08
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class InventoryPresetUpdateMessage : Message
{

public const ushort Id = 6171;
public override ushort MessageId
{
    get { return Id; }
}

public Types.Preset preset;
        

public InventoryPresetUpdateMessage()
{
}

public InventoryPresetUpdateMessage(Types.Preset preset)
        {
            this.preset = preset;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

preset.Serialize(writer);
            

}

public override void Deserialize(ICustomDataInput reader)
{

preset = new Types.Preset();
            preset.Deserialize(reader);
            

}


}


}