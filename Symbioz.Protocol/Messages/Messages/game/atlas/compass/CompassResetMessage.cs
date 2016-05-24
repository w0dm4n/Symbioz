


















// Generated on 06/04/2015 18:44:12
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class CompassResetMessage : Message
{

public const ushort Id = 5584;
public override ushort MessageId
{
    get { return Id; }
}

public sbyte type;
        

public CompassResetMessage()
{
}

public CompassResetMessage(sbyte type)
        {
            this.type = type;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteSByte(type);
            

}

public override void Deserialize(ICustomDataInput reader)
{

type = reader.ReadSByte();
            if (type < 0)
                throw new Exception("Forbidden value on type = " + type + ", it doesn't respect the following condition : type < 0");
            

}


}


}