


















// Generated on 06/04/2015 18:44:13
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class CharacterSelectedForceMessage : Message
{

public const ushort Id = 6068;
public override ushort MessageId
{
    get { return Id; }
}

public int id;
        

public CharacterSelectedForceMessage()
{
}

public CharacterSelectedForceMessage(int id)
        {
            this.id = id;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteInt(id);
            

}

public override void Deserialize(ICustomDataInput reader)
{

id = reader.ReadInt();
            if ((id < 1) || (id > 2147483647))
                throw new Exception("Forbidden value on id = " + id + ", it doesn't respect the following condition : (id < 1) || (id > 2147483647)");
            

}


}


}