


















// Generated on 06/04/2015 18:45:12
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class PrismsListMessage : Message
{

public const ushort Id = 6440;
public override ushort MessageId
{
    get { return Id; }
}

public IEnumerable<Types.PrismSubareaEmptyInfo> prisms;
        

public PrismsListMessage()
{
}

public PrismsListMessage(IEnumerable<Types.PrismSubareaEmptyInfo> prisms)
        {
            this.prisms = prisms;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteUShort((ushort)prisms.Count());
            foreach (var entry in prisms)
            {
                 writer.WriteShort(entry.TypeId);
                 entry.Serialize(writer);
            }
            

}

public override void Deserialize(ICustomDataInput reader)
{

var limit = reader.ReadUShort();
            prisms = new Types.PrismSubareaEmptyInfo[limit];
            for (int i = 0; i < limit; i++)
            {
                 (prisms as Types.PrismSubareaEmptyInfo[])[i] = Types.ProtocolTypeManager.GetInstance<Types.PrismSubareaEmptyInfo>(reader.ReadShort());
                 (prisms as Types.PrismSubareaEmptyInfo[])[i].Deserialize(reader);
            }
            

}


}


}