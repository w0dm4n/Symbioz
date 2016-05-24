


















// Generated on 06/04/2015 18:45:25
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Types
{

public class AtlasPointsInformations
{

public const short Id = 175;
public virtual short TypeId
{
    get { return Id; }
}

public sbyte type;
        public IEnumerable<Types.MapCoordinatesExtended> coords;
        

public AtlasPointsInformations()
{
}

public AtlasPointsInformations(sbyte type, IEnumerable<Types.MapCoordinatesExtended> coords)
        {
            this.type = type;
            this.coords = coords;
        }
        

public virtual void Serialize(ICustomDataOutput writer)
{

writer.WriteSByte(type);
            writer.WriteUShort((ushort)coords.Count());
            foreach (var entry in coords)
            {
                 entry.Serialize(writer);
            }
            

}

public virtual void Deserialize(ICustomDataInput reader)
{

type = reader.ReadSByte();
            if (type < 0)
                throw new Exception("Forbidden value on type = " + type + ", it doesn't respect the following condition : type < 0");
            var limit = reader.ReadUShort();
            coords = new Types.MapCoordinatesExtended[limit];
            for (int i = 0; i < limit; i++)
            {
                 (coords as Types.MapCoordinatesExtended[])[i] = new Types.MapCoordinatesExtended();
                 (coords as Types.MapCoordinatesExtended[])[i].Deserialize(reader);
            }
            

}


}


}