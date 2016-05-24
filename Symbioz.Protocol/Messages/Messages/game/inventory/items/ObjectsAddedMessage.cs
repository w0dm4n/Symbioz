


















// Generated on 06/04/2015 18:45:06
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class ObjectsAddedMessage : Message
{

public const ushort Id = 6033;
public override ushort MessageId
{
    get { return Id; }
}

public IEnumerable<Types.ObjectItem> @object;
        

public ObjectsAddedMessage()
{
}

public ObjectsAddedMessage(IEnumerable<Types.ObjectItem> @object)
        {
            this.@object = @object;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteUShort((ushort)@object.Count());
            foreach (var entry in @object)
            {
                 entry.Serialize(writer);
            }
            

}

public override void Deserialize(ICustomDataInput reader)
{

var limit = reader.ReadUShort();
            @object = new Types.ObjectItem[limit];
            for (int i = 0; i < limit; i++)
            {
                 (@object as Types.ObjectItem[])[i] = new Types.ObjectItem();
                 (@object as Types.ObjectItem[])[i].Deserialize(reader);
            }
            

}


}


}