


















// Generated on 06/04/2015 18:44:29
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class JobCrafterDirectoryAddMessage : Message
{

public const ushort Id = 5651;
public override ushort MessageId
{
    get { return Id; }
}

public Types.JobCrafterDirectoryListEntry listEntry;
        

public JobCrafterDirectoryAddMessage()
{
}

public JobCrafterDirectoryAddMessage(Types.JobCrafterDirectoryListEntry listEntry)
        {
            this.listEntry = listEntry;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

listEntry.Serialize(writer);
            

}

public override void Deserialize(ICustomDataInput reader)
{

listEntry = new Types.JobCrafterDirectoryListEntry();
            listEntry.Deserialize(reader);
            

}


}


}