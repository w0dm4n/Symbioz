


















// Generated on 06/04/2015 18:45:16
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class ClientKeyMessage : Message
{

public const ushort Id = 5607;
public override ushort MessageId
{
    get { return Id; }
}

public string key;
        

public ClientKeyMessage()
{
}

public ClientKeyMessage(string key)
        {
            this.key = key;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteUTF(key);
            

}

public override void Deserialize(ICustomDataInput reader)
{

key = reader.ReadUTF();
            

}


}


}