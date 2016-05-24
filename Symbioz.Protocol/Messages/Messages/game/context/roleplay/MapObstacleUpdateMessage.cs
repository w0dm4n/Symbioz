


















// Generated on 06/04/2015 18:44:25
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class MapObstacleUpdateMessage : Message
{

public const ushort Id = 6051;
public override ushort MessageId
{
    get { return Id; }
}

public IEnumerable<Types.MapObstacle> obstacles;
        

public MapObstacleUpdateMessage()
{
}

public MapObstacleUpdateMessage(IEnumerable<Types.MapObstacle> obstacles)
        {
            this.obstacles = obstacles;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteUShort((ushort)obstacles.Count());
            foreach (var entry in obstacles)
            {
                 entry.Serialize(writer);
            }
            

}

public override void Deserialize(ICustomDataInput reader)
{

var limit = reader.ReadUShort();
            obstacles = new Types.MapObstacle[limit];
            for (int i = 0; i < limit; i++)
            {
                 (obstacles as Types.MapObstacle[])[i] = new Types.MapObstacle();
                 (obstacles as Types.MapObstacle[])[i].Deserialize(reader);
            }
            

}


}


}