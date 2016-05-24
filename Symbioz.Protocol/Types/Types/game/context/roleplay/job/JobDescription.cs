


















// Generated on 06/04/2015 18:45:29
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Types
{

public class JobDescription
{

public const short Id = 101;
public virtual short TypeId
{
    get { return Id; }
}

public sbyte jobId;
        public IEnumerable<Types.SkillActionDescription> skills;
        

public JobDescription()
{
}

public JobDescription(sbyte jobId, IEnumerable<Types.SkillActionDescription> skills)
        {
            this.jobId = jobId;
            this.skills = skills;
        }
        

public virtual void Serialize(ICustomDataOutput writer)
{

writer.WriteSByte(jobId);
            writer.WriteUShort((ushort)skills.Count());
            foreach (var entry in skills)
            {
                 writer.WriteShort(entry.TypeId);
                 entry.Serialize(writer);
            }
            

}

public virtual void Deserialize(ICustomDataInput reader)
{

jobId = reader.ReadSByte();
            if (jobId < 0)
                throw new Exception("Forbidden value on jobId = " + jobId + ", it doesn't respect the following condition : jobId < 0");
            var limit = reader.ReadUShort();
            skills = new Types.SkillActionDescription[limit];
            for (int i = 0; i < limit; i++)
            {
                 (skills as Types.SkillActionDescription[])[i] = Types.ProtocolTypeManager.GetInstance<Types.SkillActionDescription>(reader.ReadShort());
                 (skills as Types.SkillActionDescription[])[i].Deserialize(reader);
            }
            

}


}


}