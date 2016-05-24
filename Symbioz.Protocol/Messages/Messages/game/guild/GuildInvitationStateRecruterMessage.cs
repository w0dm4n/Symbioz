


















// Generated on 06/04/2015 18:44:44
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class GuildInvitationStateRecruterMessage : Message
{

public const ushort Id = 5563;
public override ushort MessageId
{
    get { return Id; }
}

public string recrutedName;
        public sbyte invitationState;
        

public GuildInvitationStateRecruterMessage()
{
}

public GuildInvitationStateRecruterMessage(string recrutedName, sbyte invitationState)
        {
            this.recrutedName = recrutedName;
            this.invitationState = invitationState;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteUTF(recrutedName);
            writer.WriteSByte(invitationState);
            

}

public override void Deserialize(ICustomDataInput reader)
{

recrutedName = reader.ReadUTF();
            invitationState = reader.ReadSByte();
            if (invitationState < 0)
                throw new Exception("Forbidden value on invitationState = " + invitationState + ", it doesn't respect the following condition : invitationState < 0");
            

}


}


}