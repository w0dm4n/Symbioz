


















// Generated on 06/04/2015 18:44:27
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class EmotePlayMessage : EmotePlayAbstractMessage
{

public const ushort Id = 5683;
public override ushort MessageId
{
    get { return Id; }
}

public int actorId;
        public int accountId;
        

public EmotePlayMessage()
{
}

public EmotePlayMessage(byte emoteId, double emoteStartTime, int actorId, int accountId)
         : base(emoteId, emoteStartTime)
        {
            this.actorId = actorId;
            this.accountId = accountId;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteInt(actorId);
            writer.WriteInt(accountId);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            actorId = reader.ReadInt();
            accountId = reader.ReadInt();
            if (accountId < 0)
                throw new Exception("Forbidden value on accountId = " + accountId + ", it doesn't respect the following condition : accountId < 0");
            

}


}


}