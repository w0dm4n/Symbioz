


















// Generated on 06/04/2015 18:44:13
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class CharacterSelectionWithRemodelMessage : CharacterSelectionMessage
{

public const ushort Id = 6549;
public override ushort MessageId
{
    get { return Id; }
}

public Types.RemodelingInformation remodel;
        

public CharacterSelectionWithRemodelMessage()
{
}

public CharacterSelectionWithRemodelMessage(int id, Types.RemodelingInformation remodel)
         : base(id)
        {
            this.remodel = remodel;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            remodel.Serialize(writer);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            remodel = new Types.RemodelingInformation();
            remodel.Deserialize(reader);
            

}


}


}