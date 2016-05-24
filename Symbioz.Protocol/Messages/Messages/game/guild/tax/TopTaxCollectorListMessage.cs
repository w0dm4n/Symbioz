


















// Generated on 06/04/2015 18:44:47
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class TopTaxCollectorListMessage : AbstractTaxCollectorListMessage
{

public const ushort Id = 6565;
public override ushort MessageId
{
    get { return Id; }
}

public bool isDungeon;
        

public TopTaxCollectorListMessage()
{
}

public TopTaxCollectorListMessage(IEnumerable<Types.TaxCollectorInformations> informations, bool isDungeon)
         : base(informations)
        {
            this.isDungeon = isDungeon;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteBoolean(isDungeon);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            isDungeon = reader.ReadBoolean();
            

}


}


}