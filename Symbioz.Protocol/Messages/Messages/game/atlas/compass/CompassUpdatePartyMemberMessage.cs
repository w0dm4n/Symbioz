


















// Generated on 06/04/2015 18:44:12
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class CompassUpdatePartyMemberMessage : CompassUpdateMessage
{

public const ushort Id = 5589;
public override ushort MessageId
{
    get { return Id; }
}

public uint memberId;
        

public CompassUpdatePartyMemberMessage()
{
}

public CompassUpdatePartyMemberMessage(sbyte type, Types.MapCoordinates coords, uint memberId)
         : base(type, coords)
        {
            this.memberId = memberId;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteVarUhInt(memberId);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            memberId = reader.ReadVarUhInt();
            if (memberId < 0)
                throw new Exception("Forbidden value on memberId = " + memberId + ", it doesn't respect the following condition : memberId < 0");
            

}


}


}