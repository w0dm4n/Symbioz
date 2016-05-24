


















// Generated on 06/04/2015 18:45:24
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Types
{

public class TaxCollectorStaticInformations
{

public const short Id = 147;
public virtual short TypeId
{
    get { return Id; }
}

public ushort firstNameId;
        public ushort lastNameId;
        public Types.GuildInformations guildIdentity;
        

public TaxCollectorStaticInformations()
{
}

public TaxCollectorStaticInformations(ushort firstNameId, ushort lastNameId, Types.GuildInformations guildIdentity)
        {
            this.firstNameId = firstNameId;
            this.lastNameId = lastNameId;
            this.guildIdentity = guildIdentity;
        }
        

public virtual void Serialize(ICustomDataOutput writer)
{

writer.WriteVarUhShort(firstNameId);
            writer.WriteVarUhShort(lastNameId);
            guildIdentity.Serialize(writer);
            

}

public virtual void Deserialize(ICustomDataInput reader)
{

firstNameId = reader.ReadVarUhShort();
            if (firstNameId < 0)
                throw new Exception("Forbidden value on firstNameId = " + firstNameId + ", it doesn't respect the following condition : firstNameId < 0");
            lastNameId = reader.ReadVarUhShort();
            if (lastNameId < 0)
                throw new Exception("Forbidden value on lastNameId = " + lastNameId + ", it doesn't respect the following condition : lastNameId < 0");
            guildIdentity = new Types.GuildInformations();
            guildIdentity.Deserialize(reader);
            

}


}


}