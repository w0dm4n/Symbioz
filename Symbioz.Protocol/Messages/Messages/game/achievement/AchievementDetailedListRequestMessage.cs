


















// Generated on 06/04/2015 18:44:04
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class AchievementDetailedListRequestMessage : Message
{

public const ushort Id = 6357;
public override ushort MessageId
{
    get { return Id; }
}

public ushort categoryId;
        

public AchievementDetailedListRequestMessage()
{
}

public AchievementDetailedListRequestMessage(ushort categoryId)
        {
            this.categoryId = categoryId;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteVarUhShort(categoryId);
            

}

public override void Deserialize(ICustomDataInput reader)
{

categoryId = reader.ReadVarUhShort();
            if (categoryId < 0)
                throw new Exception("Forbidden value on categoryId = " + categoryId + ", it doesn't respect the following condition : categoryId < 0");
            

}


}


}