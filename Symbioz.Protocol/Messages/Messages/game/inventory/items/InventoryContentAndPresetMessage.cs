


















// Generated on 06/04/2015 18:45:03
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class InventoryContentAndPresetMessage : InventoryContentMessage
{

public const ushort Id = 6162;
public override ushort MessageId
{
    get { return Id; }
}

public IEnumerable<Types.Preset> presets;
        

public InventoryContentAndPresetMessage()
{
}

public InventoryContentAndPresetMessage(IEnumerable<Types.ObjectItem> objects, uint kamas, IEnumerable<Types.Preset> presets)
         : base(objects, kamas)
        {
            this.presets = presets;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteUShort((ushort)presets.Count());
            foreach (var entry in presets)
            {
                 entry.Serialize(writer);
            }
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            var limit = reader.ReadUShort();
            presets = new Types.Preset[limit];
            for (int i = 0; i < limit; i++)
            {
                 (presets as Types.Preset[])[i] = new Types.Preset();
                 (presets as Types.Preset[])[i].Deserialize(reader);
            }
            

}


}


}