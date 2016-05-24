


















// Generated on 06/04/2015 18:44:10
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class AllianceCreationResultMessage : Message
{

public const ushort Id = 6391;
public override ushort MessageId
{
    get { return Id; }
}

public sbyte result;
        

public AllianceCreationResultMessage()
{
}

public AllianceCreationResultMessage(sbyte result)
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