


















// Generated on 06/04/2015 18:45:34
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Types
{

public class TaxCollectorWaitingForHelpInformations : TaxCollectorComplementaryInformations
{

public const short Id = 447;
public override short TypeId
{
    get { return Id; }
}

public Types.ProtectedEntityWaitingForHelpInfo waitingForHelpInfo;
        

public TaxCollectorWaitingForHelpInformations()
{
}

public TaxCollectorWaitingForHelpInformations(Types.ProtectedEntityWaitingForHelpInfo waitingForHelpInfo)
        {
            this.waitingForHelpInfo = waitingForHelpInfo;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            waitingForHelpInfo.Serialize(writer);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            waitingForHelpInfo = new Types.ProtectedEntityWaitingForHelpInfo();
            waitingForHelpInfo.Deserialize(reader);
            

}


}


}