


















// Generated on 06/04/2015 18:44:13
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class CharacterFirstSelectionMessage : CharacterSelectionMessage
{

public const ushort Id = 6084;
public override ushort MessageId
{
    get { return Id; }
}

public bool doTutorial;
        

public CharacterFirstSelectionMessage()
{
}

public CharacterFirstSelectionMessage(int id, bool doTutorial)
         : base(id)
        {
            this.doTutorial = doTutorial;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteBoolean(doTutorial);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            doTutorial = reader.ReadBoolean();
            

}


}


}