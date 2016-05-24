


















// Generated on 06/04/2015 18:44:11
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class AllianceVersatileInfoListMessage : Message
{

public const ushort Id = 6436;
public override ushort MessageId
{
    get { return Id; }
}

public IEnumerable<Types.AllianceVersatileInformations> alliances;
        

public AllianceVersatileInfoListMessage()
{
}

public AllianceVersatileInfoListMessage(IEnumerable<Types.AllianceVersatileInformations> alliances)
        {
            this.alliances = alliances;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteUShort((ushort)alliances.Count());
            foreach (var entry in alliances)
            {
                 entry.Serialize(writer);
            }
            

}

public override void Deserialize(ICustomDataInput reader)
{

var limit = reader.ReadUShort();
            alliances = new Types.AllianceVersatileInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 (alliances as Types.AllianceVersatileInformations[])[i] = new Types.AllianceVersatileInformations();
                 (alliances as Types.AllianceVersatileInformations[])[i].Deserialize(reader);
            }
            

}


}


}