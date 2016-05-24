


















// Generated on 06/04/2015 18:44:53
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class ExchangeMountsPaddockAddMessage : Message
{

public const ushort Id = 6561;
public override ushort MessageId
{
    get { return Id; }
}

public IEnumerable<Types.MountClientData> mountDescription;
        

public ExchangeMountsPaddockAddMessage()
{
}

public ExchangeMountsPaddockAddMessage(IEnumerable<Types.MountClientData> mountDescription)
        {
            this.mountDescription = mountDescription;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteUShort((ushort)mountDescription.Count());
            foreach (var entry in mountDescription)
            {
                 entry.Serialize(writer);
            }
            

}

public override void Deserialize(ICustomDataInput reader)
{

var limit = reader.ReadUShort();
            mountDescription = new Types.MountClientData[limit];
            for (int i = 0; i < limit; i++)
            {
                 (mountDescription as Types.MountClientData[])[i] = new Types.MountClientData();
                 (mountDescription as Types.MountClientData[])[i].Deserialize(reader);
            }
            

}


}


}