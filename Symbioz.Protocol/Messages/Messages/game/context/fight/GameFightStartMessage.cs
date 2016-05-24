


















// Generated on 06/04/2015 18:44:21
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class GameFightStartMessage : Message
{

public const ushort Id = 712;
public override ushort MessageId
{
    get { return Id; }
}

public IEnumerable<Types.Idol> idols;
        

public GameFightStartMessage()
{
}

public GameFightStartMessage(IEnumerable<Types.Idol> idols)
        {
            this.idols = idols;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteUShort((ushort)idols.Count());
            foreach (var entry in idols)
            {
                 entry.Serialize(writer);
            }
            

}

public override void Deserialize(ICustomDataInput reader)
{

var limit = reader.ReadUShort();
            idols = new Types.Idol[limit];
            for (int i = 0; i < limit; i++)
            {
                 (idols as Types.Idol[])[i] = new Types.Idol();
                 (idols as Types.Idol[])[i].Deserialize(reader);
            }
            

}


}


}