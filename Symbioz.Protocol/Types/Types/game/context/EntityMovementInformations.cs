


















// Generated on 06/04/2015 18:45:23
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Types
{

public class EntityMovementInformations
{

public const short Id = 63;
public virtual short TypeId
{
    get { return Id; }
}

public int id;
        public IEnumerable<sbyte> steps;
        

public EntityMovementInformations()
{
}

public EntityMovementInformations(int id, IEnumerable<sbyte> steps)
        {
            this.id = id;
            this.steps = steps;
        }
        

public virtual void Serialize(ICustomDataOutput writer)
{

writer.WriteInt(id);
            writer.WriteUShort((ushort)steps.Count());
            foreach (var entry in steps)
            {
                 writer.WriteSByte(entry);
            }
            

}

public virtual void Deserialize(ICustomDataInput reader)
{

id = reader.ReadInt();
            var limit = reader.ReadUShort();
            steps = new sbyte[limit];
            for (int i = 0; i < limit; i++)
            {
                 (steps as sbyte[])[i] = reader.ReadSByte();
            }
            

}


}


}