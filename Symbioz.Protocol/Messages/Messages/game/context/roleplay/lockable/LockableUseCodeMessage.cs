


















// Generated on 06/04/2015 18:44:31
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class LockableUseCodeMessage : Message
{

public const ushort Id = 5667;
public override ushort MessageId
{
    get { return Id; }
}

public string code;
        

public LockableUseCodeMessage()
{
}

public LockableUseCodeMessage(string code)
        {
            this.code = code;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteUTF(code);
            

}

public override void Deserialize(ICustomDataInput reader)
{

code = reader.ReadUTF();
            

}


}


}