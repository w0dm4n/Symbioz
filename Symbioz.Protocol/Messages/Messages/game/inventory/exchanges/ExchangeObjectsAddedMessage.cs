


















// Generated on 06/04/2015 18:44:54
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class ExchangeObjectsAddedMessage : ExchangeObjectMessage
{

public const ushort Id = 6535;
public override ushort MessageId
{
    get { return Id; }
}

public IEnumerable<Types.ObjectItem> @object;
        

public ExchangeObjectsAddedMessage()
{
}

public ExchangeObjectsAddedMessage(bool remote, IEnumerable<Types.ObjectItem> @object)
         : base(remote)
        {
            this.@object = @object;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteUShort((ushort)@object.Count());
            foreach (var entry in @object)
            {
                 entry.Serialize(writer);
            }
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
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