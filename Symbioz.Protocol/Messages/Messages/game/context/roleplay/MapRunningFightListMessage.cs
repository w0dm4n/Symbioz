


















// Generated on 06/04/2015 18:44:25
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class MapRunningFightListMessage : Message
{

public const ushort Id = 5743;
public override ushort MessageId
{
    get { return Id; }
}

public IEnumerable<Types.FightExternalInformations> fights;
        

public MapRunningFightListMessage()
{
}

public MapRunningFightListMessage(IEnumerable<Types.FightExternalInformations> fights)
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
            fights = new Types.FightExternalInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 (fights as Types.FightExternalInformations[])[i] = new Types.FightExternalInformations();
                 (fights as Types.FightExternalInformations[])[i].Deserialize(reader);
            }
            

}


}


}