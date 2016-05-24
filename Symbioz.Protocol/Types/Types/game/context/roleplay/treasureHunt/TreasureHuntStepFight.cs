


















// Generated on 06/04/2015 18:45:32
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Types
{

public class TreasureHuntStepFight : TreasureHuntStep
{

public const short Id = 462;
public override short TypeId
{
    get { return Id; }
}



public TreasureHuntStepFight()
{
}



public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            

}


}


}