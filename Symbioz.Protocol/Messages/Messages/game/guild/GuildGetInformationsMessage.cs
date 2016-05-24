


















// Generated on 06/04/2015 18:44:44
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class GuildGetInformationsMessage : Message
{

public const ushort Id = 5550;
public override ushort MessageId
{
    get { return Id; }
}

public sbyte infoType;
        

public GuildGetInformationsMessage()
{
}

public GuildGetInformationsMessage(sbyte infoType)
        {
            this.infoType = infoType;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteSByte(infoType);
            

}

public override void Deserialize(ICustomDataInput reader)
{

infoType = reader.ReadSByte();
            if (infoType < 0)
                throw new Exception("Forbidden value on infoType = " + infoType + ", it doesn't respect the following condition : infoType < 0");
            

}


}


}