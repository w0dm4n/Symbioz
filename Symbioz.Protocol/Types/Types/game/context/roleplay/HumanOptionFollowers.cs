


















// Generated on 06/04/2015 18:45:27
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Types
{

public class HumanOptionFollowers : HumanOption
{

public const short Id = 410;
public override short TypeId
{
    get { return Id; }
}

public IEnumerable<Types.IndexedEntityLook> followingCharactersLook;
        

public HumanOptionFollowers()
{
}

public HumanOptionFollowers(IEnumerable<Types.IndexedEntityLook> followingCharactersLook)
        {
            this.followingCharactersLook = followingCharactersLook;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteUShort((ushort)followingCharactersLook.Count());
            foreach (var entry in followingCharactersLook)
            {
                 entry.Serialize(writer);
            }
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            var limit = reader.ReadUShort();
            followingCharactersLook = new Types.IndexedEntityLook[limit];
            for (int i = 0; i < limit; i++)
            {
                 (followingCharactersLook as Types.IndexedEntityLook[])[i] = new Types.IndexedEntityLook();
                 (followingCharactersLook as Types.IndexedEntityLook[])[i].Deserialize(reader);
            }
            

}


}


}