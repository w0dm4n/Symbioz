


















// Generated on 06/04/2015 18:44:30
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class JobExperienceMultiUpdateMessage : Message
{

public const ushort Id = 5809;
public override ushort MessageId
{
    get { return Id; }
}

public IEnumerable<Types.JobExperience> experiencesUpdate;
        

public JobExperienceMultiUpdateMessage()
{
}

public JobExperienceMultiUpdateMessage(IEnumerable<Types.JobExperience> experiencesUpdate)
        {
            this.experiencesUpdate = experiencesUpdate;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteUShort((ushort)experiencesUpdate.Count());
            foreach (var entry in experiencesUpdate)
            {
                 entry.Serialize(writer);
            }
            

}

public override void Deserialize(ICustomDataInput reader)
{

var limit = reader.ReadUShort();
            experiencesUpdate = new Types.JobExperience[limit];
            for (int i = 0; i < limit; i++)
            {
                 (experiencesUpdate as Types.JobExperience[])[i] = new Types.JobExperience();
                 (experiencesUpdate as Types.JobExperience[])[i].Deserialize(reader);
            }
            

}


}


}