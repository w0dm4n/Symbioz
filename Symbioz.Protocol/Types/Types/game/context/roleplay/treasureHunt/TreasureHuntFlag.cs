


















// Generated on 06/04/2015 18:45:31
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Types
{

public class TreasureHuntFlag
{

public const short Id = 473;
public virtual short TypeId
{
    get { return Id; }
}

public int mapId;
        public sbyte state;
        

public TreasureHuntFlag()
{
}

public TreasureHuntFlag(int mapId, sbyte state)
        {
            this.mapId = mapId;
            this.state = state;
        }
        

public virtual void Serialize(ICustomDataOutput writer)
{

writer.WriteInt(mapId);
            writer.WriteSByte(state);
            

}

public virtual void Deserialize(ICustomDataInput reader)
{

mapId = reader.ReadInt();
            state = reader.ReadSByte();
            if (state < 0)
                throw new Exception("Forbidden value on state = " + state + ", it doesn't respect the following condition : state < 0");
            

}


}


}