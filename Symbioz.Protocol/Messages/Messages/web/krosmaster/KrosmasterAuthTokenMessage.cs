


















// Generated on 06/04/2015 18:45:17
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class KrosmasterAuthTokenMessage : Message
{

public const ushort Id = 6351;
public override ushort MessageId
{
    get { return Id; }
}

public string token;
        

public KrosmasterAuthTokenMessage()
{
}

public KrosmasterAuthTokenMessage(string token)
        {
            this.token = token;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteUTF(token);
            

}

public override void Deserialize(ICustomDataInput reader)
{

token = reader.ReadUTF();
            

}


}


}