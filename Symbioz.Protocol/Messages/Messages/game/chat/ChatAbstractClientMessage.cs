


















// Generated on 06/04/2015 18:44:16
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class ChatAbstractClientMessage : Message
{

public const ushort Id = 850;
public override ushort MessageId
{
    get { return Id; }
}

public string content;
        

public ChatAbstractClientMessage()
{
}

public ChatAbstractClientMessage(string content)
        {
            this.content = content;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteUTF(content);
            

}

public override void Deserialize(ICustomDataInput reader)
{

content = reader.ReadUTF();
            

}


}


}