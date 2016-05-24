


















// Generated on 06/04/2015 18:44:29
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class JobCrafterDirectoryListMessage : Message
{

public const ushort Id = 6046;
public override ushort MessageId
{
    get { return Id; }
}

public IEnumerable<Types.JobCrafterDirectoryListEntry> listEntries;
        

public JobCrafterDirectoryListMessage()
{
}

public JobCrafterDirectoryListMessage(IEnumerable<Types.JobCrafterDirectoryListEntry> listEntries)
        {
            this.listEntries = listEntries;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteUShort((ushort)listEntries.Count());
            foreach (var entry in listEntries)
            {
                 entry.Serialize(writer);
            }
            

}

public override void Deserialize(ICustomDataInput reader)
{

var limit = reader.ReadUShort();
            listEntries = new Types.JobCrafterDirectoryListEntry[limit];
            for (int i = 0; i < limit; i++)
            {
                 (listEntries as Types.JobCrafterDirectoryListEntry[])[i] = new Types.JobCrafterDirectoryListEntry();
                 (listEntries as Types.JobCrafterDirectoryListEntry[])[i].Deserialize(reader);
            }
            

}


}


}