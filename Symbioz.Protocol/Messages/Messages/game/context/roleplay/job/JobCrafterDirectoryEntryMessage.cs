


















// Generated on 06/04/2015 18:44:29
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class JobCrafterDirectoryEntryMessage : Message
{

public const ushort Id = 6044;
public override ushort MessageId
{
    get { return Id; }
}

public Types.JobCrafterDirectoryEntryPlayerInfo playerInfo;
        public IEnumerable<Types.JobCrafterDirectoryEntryJobInfo> jobInfoList;
        public Types.EntityLook playerLook;
        

public JobCrafterDirectoryEntryMessage()
{
}

public JobCrafterDirectoryEntryMessage(Types.JobCrafterDirectoryEntryPlayerInfo playerInfo, IEnumerable<Types.JobCrafterDirectoryEntryJobInfo> jobInfoList, Types.EntityLook playerLook)
        {
            this.playerInfo = playerInfo;
            this.jobInfoList = jobInfoList;
            this.playerLook = playerLook;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

playerInfo.Serialize(writer);
            writer.WriteUShort((ushort)jobInfoList.Count());
            foreach (var entry in jobInfoList)
            {
                 entry.Serialize(writer);
            }
            playerLook.Serialize(writer);
            

}

public override void Deserialize(ICustomDataInput reader)
{

playerInfo = new Types.JobCrafterDirectoryEntryPlayerInfo();
            playerInfo.Deserialize(reader);
            var limit = reader.ReadUShort();
            jobInfoList = new Types.JobCrafterDirectoryEntryJobInfo[limit];
            for (int i = 0; i < limit; i++)
            {
                 (jobInfoList as Types.JobCrafterDirectoryEntryJobInfo[])[i] = new Types.JobCrafterDirectoryEntryJobInfo();
                 (jobInfoList as Types.JobCrafterDirectoryEntryJobInfo[])[i].Deserialize(reader);
            }
            playerLook = new Types.EntityLook();
            playerLook.Deserialize(reader);
            

}


}


}