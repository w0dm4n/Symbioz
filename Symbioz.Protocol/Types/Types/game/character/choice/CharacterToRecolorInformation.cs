


















// Generated on 06/04/2015 18:45:23
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Types
{

public class CharacterToRecolorInformation : AbstractCharacterToRefurbishInformation
{

public const short Id = 212;
public override short TypeId
{
    get { return Id; }
}



public CharacterToRecolorInformation()
{
}

public CharacterToRecolorInformation(uint id, IEnumerable<int> colors, uint cosmeticId)
         : base(id, colors, cosmeticId)
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