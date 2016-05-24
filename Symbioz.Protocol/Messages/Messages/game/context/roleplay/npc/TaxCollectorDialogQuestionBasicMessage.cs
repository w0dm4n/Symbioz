


















// Generated on 06/04/2015 18:44:32
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class TaxCollectorDialogQuestionBasicMessage : Message
{

public const ushort Id = 5619;
public override ushort MessageId
{
    get { return Id; }
}

public Types.BasicGuildInformations guildInfo;
        

public TaxCollectorDialogQuestionBasicMessage()
{
}

public TaxCollectorDialogQuestionBasicMessage(Types.BasicGuildInformations guildInfo)
        {
            this.guildInfo = guildInfo;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

guildInfo.Serialize(writer);
            

}

public override void Deserialize(ICustomDataInput reader)
{

guildInfo = new Types.BasicGuildInformations();
            guildInfo.Deserialize(reader);
            

}


}


}