


















// Generated on 06/04/2015 18:45:25
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Types
{

public class GameFightFighterTaxCollectorLightInformations : GameFightFighterLightInformations
{

public const short Id = 457;
public override short TypeId
{
    get { return Id; }
}

public ushort firstNameId;
        public ushort lastNameId;
        

public GameFightFighterTaxCollectorLightInformations()
{
}

public GameFightFighterTaxCollectorLightInformations(bool sex, bool alive, int id, sbyte wave, ushort level, sbyte breed, ushort firstNameId, ushort lastNameId)
         : base(sex, alive, id, wave, level, breed)
        {
            this.firstNameId = firstNameId;
            this.lastNameId = lastNameId;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteVarUhShort(firstNameId);
            writer.WriteVarUhShort(lastNameId);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            firstNameId = reader.ReadVarUhShort();
            if (firstNameId < 0)
                throw new Exception("Forbidden value on firstNameId = " + firstNameId + ", it doesn't respect the following condition : firstNameId < 0");
            lastNameId = reader.ReadVarUhShort();
            if (lastNameId < 0)
                throw new Exception("Forbidden value on lastNameId = " + lastNameId + ", it doesn't respect the following condition : lastNameId < 0");
            

}


}


}