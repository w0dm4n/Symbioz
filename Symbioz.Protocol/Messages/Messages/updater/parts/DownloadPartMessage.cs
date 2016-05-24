


















// Generated on 06/04/2015 18:45:17
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class DownloadPartMessage : Message
{

public const ushort Id = 1503;
public override ushort MessageId
{
    get { return Id; }
}

public string id;
        

public DownloadPartMessage()
{
}

public DownloadPartMessage(string id)
        {
            this.id = id;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteUTF(id);
            

}

public override void Deserialize(ICustomDataInput reader)
{

id = reader.ReadUTF();
            

}


}


}