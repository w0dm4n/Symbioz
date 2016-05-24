


















// Generated on 06/04/2015 18:45:26
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Types
{

public class BasicNamedAllianceInformations : BasicAllianceInformations
{

public const short Id = 418;
public override short TypeId
{
    get { return Id; }
}

public string allianceName;
        

public BasicNamedAllianceInformations()
{
}

public BasicNamedAllianceInformations(uint allianceId, string allianceTag, string allianceName)
         : base(allianceId, allianceTag)
        {
            this.allianceName = allianceName;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteUTF(allianceName);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            allianceName = reader.ReadUTF();
            

}


}


}