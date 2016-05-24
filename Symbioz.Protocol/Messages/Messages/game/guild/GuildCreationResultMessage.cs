


















// Generated on 06/04/2015 18:44:43
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class GuildCreationResultMessage : Message
{

public const ushort Id = 5554;
public override ushort MessageId
{
    get { return Id; }
}

public sbyte result;
        

public GuildCreationResultMessage()
{
}

public GuildCreationResultMessage(sbyte result)
        {
            this.result = result;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteSByte(result);
            

}

public override void Deserialize(ICustomDataInput reader)
{

result = reader.ReadSByte();
            if (result < 0)
                throw new Exception("Forbidden value on result = " + result + ", it doesn't respect the following condition : result < 0");
            

}


}


}