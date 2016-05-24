


















// Generated on 06/04/2015 18:44:43
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class GuildCharacsUpgradeRequestMessage : Message
{

public const ushort Id = 5706;
public override ushort MessageId
{
    get { return Id; }
}

public sbyte charaTypeTarget;
        

public GuildCharacsUpgradeRequestMessage()
{
}

public GuildCharacsUpgradeRequestMessage(sbyte charaTypeTarget)
        {
            this.charaTypeTarget = charaTypeTarget;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteSByte(charaTypeTarget);
            

}

public override void Deserialize(ICustomDataInput reader)
{

charaTypeTarget = reader.ReadSByte();
            if (charaTypeTarget < 0)
                throw new Exception("Forbidden value on charaTypeTarget = " + charaTypeTarget + ", it doesn't respect the following condition : charaTypeTarget < 0");
            

}


}


}