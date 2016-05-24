


















// Generated on 06/04/2015 18:44:46
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class AbstractTaxCollectorListMessage : Message
{

public const ushort Id = 6568;
public override ushort MessageId
{
    get { return Id; }
}

public IEnumerable<Types.TaxCollectorInformations> informations;
        

public AbstractTaxCollectorListMessage()
{
}

public AbstractTaxCollectorListMessage(IEnumerable<Types.TaxCollectorInformations> informations)
        {
            this.informations = informations;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteUShort((ushort)informations.Count());
            foreach (var entry in informations)
            {
                 writer.WriteShort(entry.TypeId);
                 entry.Serialize(writer);
            }
            

}

public override void Deserialize(ICustomDataInput reader)
{

var limit = reader.ReadUShort();
            informations = new Types.TaxCollectorInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 (informations as Types.TaxCollectorInformations[])[i] = Types.ProtocolTypeManager.GetInstance<Types.TaxCollectorInformations>(reader.ReadShort());
                 (informations as Types.TaxCollectorInformations[])[i].Deserialize(reader);
            }
            

}


}


}