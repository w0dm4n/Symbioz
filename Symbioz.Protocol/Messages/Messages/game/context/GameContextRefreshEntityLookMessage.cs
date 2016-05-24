


















// Generated on 06/04/2015 18:44:18
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class GameContextRefreshEntityLookMessage : Message
{

public const ushort Id = 5637;
public override ushort MessageId
{
    get { return Id; }
}

public int id;
        public Types.EntityLook look;
        

public GameContextRefreshEntityLookMessage()
{
}

public GameContextRefreshEntityLookMessage(int id, Types.EntityLook look)
        {
            this.id = id;
            this.look = look;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteInt(id);
            look.Serialize(writer);
            

}

public override void Deserialize(ICustomDataInput reader)
{

id = reader.ReadInt();
            look = new Types.EntityLook();
            look.Deserialize(reader);
            

}


}


}