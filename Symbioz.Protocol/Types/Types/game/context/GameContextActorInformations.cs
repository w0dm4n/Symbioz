


















// Generated on 06/04/2015 18:45:23
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Types
{

public class GameContextActorInformations
{

public const short Id = 150;
public virtual short TypeId
{
    get { return Id; }
}

public int contextualId;
        public Types.EntityLook look;
        public Types.EntityDispositionInformations disposition;
        

public GameContextActorInformations()
{
}

public GameContextActorInformations(int contextualId, Types.EntityLook look, Types.EntityDispositionInformations disposition)
        {
            this.contextualId = contextualId;
            this.look = look;
            this.disposition = disposition;
        }
        

public virtual void Serialize(ICustomDataOutput writer)
{

writer.WriteInt(contextualId);
            look.Serialize(writer);
            writer.WriteShort(disposition.TypeId);
            disposition.Serialize(writer);
            

}

public virtual void Deserialize(ICustomDataInput reader)
{

contextualId = reader.ReadInt();
            look = new Types.EntityLook();
            look.Deserialize(reader);
            disposition = Types.ProtocolTypeManager.GetInstance<Types.EntityDispositionInformations>(reader.ReadShort());
            disposition.Deserialize(reader);
            

}


}


}