


















// Generated on 06/04/2015 18:44:19
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class GameMapMovementMessage : Message
{

public const ushort Id = 951;
public override ushort MessageId
{
    get { return Id; }
}

public IEnumerable<short> keyMovements;
        public int actorId;
        

public GameMapMovementMessage()
{
}

public GameMapMovementMessage(IEnumerable<short> keyMovements, int actorId)
        {
            this.keyMovements = keyMovements;
            this.actorId = actorId;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteUShort((ushort)keyMovements.Count());
            foreach (var entry in keyMovements)
            {
                 writer.WriteShort(entry);
            }
            writer.WriteInt(actorId);
            

}

public override void Deserialize(ICustomDataInput reader)
{

var limit = reader.ReadUShort();
            keyMovements = new short[limit];
            for (int i = 0; i < limit; i++)
            {
                 (keyMovements as short[])[i] = reader.ReadShort();
            }
            actorId = reader.ReadInt();
            

}


}


}