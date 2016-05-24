


















// Generated on 06/04/2015 18:44:11
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class AlliancePartialListMessage : AllianceListMessage
{

public const ushort Id = 6427;
public override ushort MessageId
{
    get { return Id; }
}



public AlliancePartialListMessage()
{
}

public AlliancePartialListMessage(IEnumerable<Types.AllianceFactSheetInformations> alliances)
         : base(alliances)
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