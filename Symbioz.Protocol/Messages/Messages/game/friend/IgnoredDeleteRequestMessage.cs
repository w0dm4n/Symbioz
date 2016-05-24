


















// Generated on 06/04/2015 18:44:43
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class IgnoredDeleteRequestMessage : Message
{

public const ushort Id = 5680;
public override ushort MessageId
{
    get { return Id; }
}

public int accountId;
        public bool session;
        

public IgnoredDeleteRequestMessage()
{
}

public IgnoredDeleteRequestMessage(int accountId, bool session)
        {
            this.accountId = accountId;
            this.session = session;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteInt(accountId);
            writer.WriteBoolean(session);
            

}

public override void Deserialize(ICustomDataInput reader)
{

accountId = reader.ReadInt();
            if (accountId < 0)
                throw new Exception("Forbidden value on accountId = " + accountId + ", it doesn't respect the following condition : accountId < 0");
            session = reader.ReadBoolean();
            

}


}


}