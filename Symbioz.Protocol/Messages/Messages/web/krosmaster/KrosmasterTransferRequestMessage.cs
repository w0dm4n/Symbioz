


















// Generated on 06/04/2015 18:45:18
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class KrosmasterTransferRequestMessage : Message
{

public const ushort Id = 6349;
public override ushort MessageId
{
    get { return Id; }
}

public string uid;
        

public KrosmasterTransferRequestMessage()
{
}

public KrosmasterTransferRequestMessage(string uid)
        {
            this.uid = uid;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteUTF(uid);
            

}

public override void Deserialize(ICustomDataInput reader)
{

uid = reader.ReadUTF();
            

}


}


}