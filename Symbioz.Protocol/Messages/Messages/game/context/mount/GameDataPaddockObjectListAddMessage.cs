


















// Generated on 06/04/2015 18:44:22
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class GameDataPaddockObjectListAddMessage : Message
{

public const ushort Id = 5992;
public override ushort MessageId
{
    get { return Id; }
}

public IEnumerable<Types.PaddockItem> paddockItemDescription;
        

public GameDataPaddockObjectListAddMessage()
{
}

public GameDataPaddockObjectListAddMessage(IEnumerable<Types.PaddockItem> paddockItemDescription)
        {
            this.paddockItemDescription = paddockItemDescription;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteUShort((ushort)paddockItemDescription.Count());
            foreach (var entry in paddockItemDescription)
            {
                 entry.Serialize(writer);
            }
            

}

public override void Deserialize(ICustomDataInput reader)
{

var limit = reader.ReadUShort();
            paddockItemDescription = new Types.PaddockItem[limit];
            for (int i = 0; i < limit; i++)
            {
                 (paddockItemDescription as Types.PaddockItem[])[i] = new Types.PaddockItem();
                 (paddockItemDescription as Types.PaddockItem[])[i].Deserialize(reader);
            }
            

}


}


}