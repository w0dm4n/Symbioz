


















// Generated on 06/04/2015 18:44:10
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class AllianceInvitedMessage : Message
{

public const ushort Id = 6397;
public override ushort MessageId
{
    get { return Id; }
}

public uint recruterId;
        public string recruterName;
        public Types.BasicNamedAllianceInformations allianceInfo;
        

public AllianceInvitedMessage()
{
}

public AllianceInvitedMessage(uint recruterId, string recruterName, Types.BasicNamedAllianceInformations allianceInfo)
        {
            this.recruterId = recruterId;
            this.recruterName = recruterName;
            this.allianceInfo = allianceInfo;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteVarUhInt(recruterId);
            writer.WriteUTF(recruterName);
            allianceInfo.Serialize(writer);
            

}

public override void Deserialize(ICustomDataInput reader)
{

recruterId = reader.ReadVarUhInt();
            if (recruterId < 0)
                throw new Exception("Forbidden value on recruterId = " + recruterId + ", it doesn't respect the following condition : recruterId < 0");
            recruterName = reader.ReadUTF();
            allianceInfo = new Types.BasicNamedAllianceInformations();
            allianceInfo.Deserialize(reader);
            

}


}


}