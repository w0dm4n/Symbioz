


















// Generated on 06/04/2015 18:45:12
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class PrismsListRegisterMessage : Message
{

public const ushort Id = 6441;
public override ushort MessageId
{
    get { return Id; }
}

public sbyte listen;
        

public PrismsListRegisterMessage()
{
}

public PrismsListRegisterMessage(sbyte listen)
        {
            this.listen = listen;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteSByte(listen);
            

}

public override void Deserialize(ICustomDataInput reader)
{

listen = reader.ReadSByte();
            if (listen < 0)
                throw new Exception("Forbidden value on listen = " + listen + ", it doesn't respect the following condition : listen < 0");
            

}


}


}