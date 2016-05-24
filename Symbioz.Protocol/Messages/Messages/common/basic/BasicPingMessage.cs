


















// Generated on 06/04/2015 18:44:02
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class BasicPingMessage : Message
{

public const ushort Id = 182;
public override ushort MessageId
{
    get { return Id; }
}

public bool quiet;
        

public BasicPingMessage()
{
}

public BasicPingMessage(bool quiet)
        {
            this.quiet = quiet;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteBoolean(quiet);
            

}

public override void Deserialize(ICustomDataInput reader)
{

quiet = reader.ReadBoolean();
            

}


}


}