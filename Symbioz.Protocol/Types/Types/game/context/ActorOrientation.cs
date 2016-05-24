


















// Generated on 06/04/2015 18:45:23
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Types
{

public class ActorOrientation
{

public const short Id = 353;
public virtual short TypeId
{
    get { return Id; }
}

public int id;
        public sbyte direction;
        

public ActorOrientation()
{
}

public ActorOrientation(int id, sbyte direction)
        {
            this.id = id;
            this.direction = direction;
        }
        

public virtual void Serialize(ICustomDataOutput writer)
{

writer.WriteInt(id);
            writer.WriteSByte(direction);
            

}

public virtual void Deserialize(ICustomDataInput reader)
{

id = reader.ReadInt();
            direction = reader.ReadSByte();
            if (direction < 0)
                throw new Exception("Forbidden value on direction = " + direction + ", it doesn't respect the following condition : direction < 0");
            

}


}


}