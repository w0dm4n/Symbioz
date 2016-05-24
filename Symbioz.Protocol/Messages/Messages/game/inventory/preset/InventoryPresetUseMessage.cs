


















// Generated on 06/04/2015 18:45:08
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class InventoryPresetUseMessage : Message
{

public const ushort Id = 6167;
public override ushort MessageId
{
    get { return Id; }
}

public sbyte presetId;
        

public InventoryPresetUseMessage()
{
}

public InventoryPresetUseMessage(sbyte presetId)
        {
            this.presetId = presetId;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteSByte(presetId);
            

}

public override void Deserialize(ICustomDataInput reader)
{

presetId = reader.ReadSByte();
            if (presetId < 0)
                throw new Exception("Forbidden value on presetId = " + presetId + ", it doesn't respect the following condition : presetId < 0");
            

}


}


}