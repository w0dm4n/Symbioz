


















// Generated on 06/04/2015 18:44:18
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class GameEntitiesDispositionMessage : Message
{

public const ushort Id = 5696;
public override ushort MessageId
{
    get { return Id; }
}

public IEnumerable<Types.IdentifiedEntityDispositionInformations> dispositions;
        

public GameEntitiesDispositionMessage()
{
}

public GameEntitiesDispositionMessage(IEnumerable<Types.IdentifiedEntityDispositionInformations> dispositions)
        {
            this.dispositions = dispositions;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteUShort((ushort)dispositions.Count());
            foreach (var entry in dispositions)
            {
                 entry.Serialize(writer);
            }
            

}

public override void Deserialize(ICustomDataInput reader)
{

var limit = reader.ReadUShort();
            dispositions = new Types.IdentifiedEntityDispositionInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 (dispositions as Types.IdentifiedEntityDispositionInformations[])[i] = new Types.IdentifiedEntityDispositionInformations();
                 (dispositions as Types.IdentifiedEntityDispositionInformations[])[i].Deserialize(reader);
            }
            

}


}


}