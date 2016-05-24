


















// Generated on 06/04/2015 18:44:30
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class JobExperienceUpdateMessage : Message
{

public const ushort Id = 5654;
public override ushort MessageId
{
    get { return Id; }
}

public Types.JobExperience experiencesUpdate;
        

public JobExperienceUpdateMessage()
{
}

public JobExperienceUpdateMessage(Types.JobExperience experiencesUpdate)
        {
            this.experiencesUpdate = experiencesUpdate;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

experiencesUpdate.Serialize(writer);
            

}

public override void Deserialize(ICustomDataInput reader)
{

experiencesUpdate = new Types.JobExperience();
            experiencesUpdate.Deserialize(reader);
            

}


}


}