


















// Generated on 06/04/2015 18:44:31
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class AllianceTaxCollectorDialogQuestionExtendedMessage : TaxCollectorDialogQuestionExtendedMessage
{

public const ushort Id = 6445;
public override ushort MessageId
{
    get { return Id; }
}

public Types.BasicNamedAllianceInformations alliance;
        

public AllianceTaxCollectorDialogQuestionExtendedMessage()
{
}

public AllianceTaxCollectorDialogQuestionExtendedMessage(Types.BasicGuildInformations guildInfo, ushort maxPods, ushort prospecting, ushort wisdom, sbyte taxCollectorsCount, int taxCollectorAttack, uint kamas, ulong experience, uint pods, uint itemsValue, Types.BasicNamedAllianceInformations alliance)
         : base(guildInfo, maxPods, prospecting, wisdom, taxCollectorsCount, taxCollectorAttack, kamas, experience, pods, itemsValue)
        {
            this.alliance = alliance;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            alliance.Serialize(writer);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            alliance = new Types.BasicNamedAllianceInformations();
            alliance.Deserialize(reader);
            

}


}


}