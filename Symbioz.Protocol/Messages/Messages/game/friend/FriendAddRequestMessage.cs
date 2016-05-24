


















// Generated on 06/04/2015 18:44:42
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class FriendAddRequestMessage : Message
{

public const ushort Id = 4004;
public override ushort MessageId
{
    get { return Id; }
}

public string name;
        

public FriendAddRequestMessage()
{
}

public FriendAddRequestMessage(string name)
        {
            this.name = name;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteUTF(name);
            

}

public override void Deserialize(ICustomDataInput reader)
{

name = reader.ReadUTF();
            

}


}


}