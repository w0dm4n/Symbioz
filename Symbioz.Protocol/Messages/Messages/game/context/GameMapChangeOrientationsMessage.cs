


















// Generated on 06/04/2015 18:44:19
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class GameMapChangeOrientationsMessage : Message
{

public const ushort Id = 6155;
public override ushort MessageId
{
    get { return Id; }
}

public IEnumerable<Types.ActorOrientation> orientations;
        

public GameMapChangeOrientationsMessage()
{
}

public GameMapChangeOrientationsMessage(IEnumerable<Types.ActorOrientation> orientations)
        {
            this.orientations = orientations;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteUShort((ushort)orientations.Count());
            foreach (var entry in orientations)
            {
                 entry.Serialize(writer);
            }
            

}

public override void Deserialize(ICustomDataInput reader)
{

var limit = reader.ReadUShort();
            orientations = new Types.ActorOrientation[limit];
            for (int i = 0; i < limit; i++)
            {
                 (orientations as Types.ActorOrientation[])[i] = new Types.ActorOrientation();
                 (orientations as Types.ActorOrientation[])[i].Deserialize(reader);
            }
            

}


}


}