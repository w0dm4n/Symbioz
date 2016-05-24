


















// Generated on 06/04/2015 18:45:37
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Types
{

public class ContentPart
{

public const short Id = 350;
public virtual short TypeId
{
    get { return Id; }
}

public string id;
        public sbyte state;
        

public ContentPart()
{
}

public ContentPart(string id, sbyte state)
        {
            this.id = id;
            this.state = state;
        }
        

public virtual void Serialize(ICustomDataOutput writer)
{

writer.WriteUTF(id);
            writer.WriteSByte(state);
            

}

public virtual void Deserialize(ICustomDataInput reader)
{

id = reader.ReadUTF();
            state = reader.ReadSByte();
            if (state < 0)
                throw new Exception("Forbidden value on state = " + state + ", it doesn't respect the following condition : state < 0");
            

}


}


}