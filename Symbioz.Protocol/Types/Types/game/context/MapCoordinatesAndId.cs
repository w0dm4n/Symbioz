


















// Generated on 06/04/2015 18:45:23
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Types
{

public class MapCoordinatesAndId : MapCoordinates
{

public const short Id = 392;
public override short TypeId
{
    get { return Id; }
}

public int mapId;
        

public MapCoordinatesAndId()
{
}

public MapCoordinatesAndId(short worldX, short worldY, int mapId)
         : base(worldX, worldY)
        {
            this.mapId = mapId;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteInt(mapId);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            mapId = reader.ReadInt();
            

}


}


}