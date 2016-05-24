


















// Generated on 06/04/2015 18:45:27
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Types
{

public class HumanInformations
{

public const short Id = 157;
public virtual short TypeId
{
    get { return Id; }
}

public Types.ActorRestrictionsInformations restrictions;
        public bool sex;
        public IEnumerable<Types.HumanOption> options;
        

public HumanInformations()
{
}

public HumanInformations(Types.ActorRestrictionsInformations restrictions, bool sex, IEnumerable<Types.HumanOption> options)
        {
            this.restrictions = restrictions;
            this.sex = sex;
            this.options = options;
        }
        

public virtual void Serialize(ICustomDataOutput writer)
{

restrictions.Serialize(writer);
            writer.WriteBoolean(sex);
            writer.WriteUShort((ushort)options.Count());
            foreach (var entry in options)
            {
                 writer.WriteShort(entry.TypeId);
                 entry.Serialize(writer);
            }
            

}

public virtual void Deserialize(ICustomDataInput reader)
{

restrictions = new Types.ActorRestrictionsInformations();
            restrictions.Deserialize(reader);
            sex = reader.ReadBoolean();
            var limit = reader.ReadUShort();
            options = new Types.HumanOption[limit];
            for (int i = 0; i < limit; i++)
            {
                 (options as Types.HumanOption[])[i] = Types.ProtocolTypeManager.GetInstance<Types.HumanOption>(reader.ReadShort());
                 (options as Types.HumanOption[])[i].Deserialize(reader);
            }
            

}


}


}