


















// Generated on 06/04/2015 18:44:43
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class IgnoredAddRequestMessage : Message
{

public const ushort Id = 5673;
public override ushort MessageId
{
    get { return Id; }
}

public string name;
        public bool session;
        

public IgnoredAddRequestMessage()
{
}

public IgnoredAddRequestMessage(string name, bool session)
        {
            this.name = name;
            this.session = session;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteUTF(name);
            writer.WriteBoolean(session);
            

}

public override void Deserialize(ICustomDataInput reader)
{

name = reader.ReadUTF();
            session = reader.ReadBoolean();
            

}


}


}