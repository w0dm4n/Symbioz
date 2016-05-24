


















// Generated on 06/04/2015 18:44:16
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class ChatClientMultiWithObjectMessage : ChatClientMultiMessage
{

public const ushort Id = 862;
public override ushort MessageId
{
    get { return Id; }
}

public IEnumerable<Types.ObjectItem> objects;
        

public ChatClientMultiWithObjectMessage()
{
}

public ChatClientMultiWithObjectMessage(string content, sbyte channel, IEnumerable<Types.ObjectItem> objects)
         : base(content, channel)
        {
            this.objects = objects;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteUShort((ushort)objects.Count());
            foreach (var entry in objects)
            {
                 entry.Serialize(writer);
            }
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            var limit = reader.ReadUShort();
            objects = new Types.ObjectItem[limit];
            for (int i = 0; i < limit; i++)
            {
                 (objects as Types.ObjectItem[])[i] = new Types.ObjectItem();
                 (objects as Types.ObjectItem[])[i].Deserialize(reader);
            }
            

}


}


}