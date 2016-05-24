


















// Generated on 06/04/2015 18:45:15
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class ClientUIOpenedByObjectMessage : ClientUIOpenedMessage
{

public const ushort Id = 6463;
public override ushort MessageId
{
    get { return Id; }
}

public uint uid;
        

public ClientUIOpenedByObjectMessage()
{
}

public ClientUIOpenedByObjectMessage(sbyte type, uint uid)
         : base(type)
        {
            this.uid = uid;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteVarUhInt(uid);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            uid = reader.ReadVarUhInt();
            if (uid < 0)
                throw new Exception("Forbidden value on uid = " + uid + ", it doesn't respect the following condition : uid < 0");
            

}


}


}