


















// Generated on 06/04/2015 18:45:27
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Types
{

public class HumanOptionAlliance : HumanOption
{

public const short Id = 425;
public override short TypeId
{
    get { return Id; }
}

public Types.AllianceInformations allianceInformations;
        public sbyte aggressable;
        

public HumanOptionAlliance()
{
}

public HumanOptionAlliance(Types.AllianceInformations allianceInformations, sbyte aggressable)
        {
            this.allianceInformations = allianceInformations;
            this.aggressable = aggressable;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            allianceInformations.Serialize(writer);
            writer.WriteSByte(aggressable);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            allianceInformations = new Types.AllianceInformations();
            allianceInformations.Deserialize(reader);
            aggressable = reader.ReadSByte();
            if (aggressable < 0)
                throw new Exception("Forbidden value on aggressable = " + aggressable + ", it doesn't respect the following condition : aggressable < 0");
            

}


}


}