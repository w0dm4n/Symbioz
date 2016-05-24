


















// Generated on 06/04/2015 18:45:12
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class PrismsInfoValidMessage : Message
{

public const ushort Id = 6451;
public override ushort MessageId
{
    get { return Id; }
}

public IEnumerable<Types.PrismFightersInformation> fights;
        

public PrismsInfoValidMessage()
{
}

public PrismsInfoValidMessage(IEnumerable<Types.PrismFightersInformation> fights)
        {
            this.fights = fights;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteUShort((ushort)fights.Count());
            foreach (var entry in fights)
            {
                 entry.Serialize(writer);
            }
            

}

public override void Deserialize(ICustomDataInput reader)
{

var limit = reader.ReadUShort();
            fights = new Types.PrismFightersInformation[limit];
            for (int i = 0; i < limit; i++)
            {
                 (fights as Types.PrismFightersInformation[])[i] = new Types.PrismFightersInformation();
                 (fights as Types.PrismFightersInformation[])[i].Deserialize(reader);
            }
            

}


}


}