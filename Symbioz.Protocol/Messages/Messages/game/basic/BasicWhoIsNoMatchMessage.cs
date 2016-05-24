


















// Generated on 06/04/2015 18:44:12
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class BasicWhoIsNoMatchMessage : Message
{

public const ushort Id = 179;
public override ushort MessageId
{
    get { return Id; }
}

public string search;
        

public BasicWhoIsNoMatchMessage()
{
}

public BasicWhoIsNoMatchMessage(string search)
        {
            this.search = search;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteUTF(search);
            

}

public override void Deserialize(ICustomDataInput reader)
{

search = reader.ReadUTF();
            

}


}


}