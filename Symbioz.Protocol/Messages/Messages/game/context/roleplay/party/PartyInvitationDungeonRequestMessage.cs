


















// Generated on 06/04/2015 18:44:36
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class PartyInvitationDungeonRequestMessage : PartyInvitationRequestMessage
{

public const ushort Id = 6245;
public override ushort MessageId
{
    get { return Id; }
}

public ushort dungeonId;
        

public PartyInvitationDungeonRequestMessage()
{
}

public PartyInvitationDungeonRequestMessage(string name, ushort dungeonId)
         : base(name)
        {
            this.dungeonId = dungeonId;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteVarUhShort(dungeonId);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            dungeonId = reader.ReadVarUhShort();
            if (dungeonId < 0)
                throw new Exception("Forbidden value on dungeonId = " + dungeonId + ", it doesn't respect the following condition : dungeonId < 0");
            

}


}


}