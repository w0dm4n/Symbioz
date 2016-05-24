


















// Generated on 06/04/2015 18:44:11
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class AllianceModificationValidMessage : Message
{

public const ushort Id = 6450;
public override ushort MessageId
{
    get { return Id; }
}

public string allianceName;
        public string allianceTag;
        public Types.GuildEmblem Alliancemblem;
        

public AllianceModificationValidMessage()
{
}

public AllianceModificationValidMessage(string allianceName, string allianceTag, Types.GuildEmblem Alliancemblem)
        {
            this.allianceName = allianceName;
            this.allianceTag = allianceTag;
            this.Alliancemblem = Alliancemblem;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteUTF(allianceName);
            writer.WriteUTF(allianceTag);
            Alliancemblem.Serialize(writer);
            

}

public override void Deserialize(ICustomDataInput reader)
{

allianceName = reader.ReadUTF();
            allianceTag = reader.ReadUTF();
            Alliancemblem = new Types.GuildEmblem();
            Alliancemblem.Deserialize(reader);
            

}


}


}