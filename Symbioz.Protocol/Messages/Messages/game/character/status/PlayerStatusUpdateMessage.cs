


















// Generated on 06/04/2015 18:44:16
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class PlayerStatusUpdateMessage : Message
{

public const ushort Id = 6386;
public override ushort MessageId
{
    get { return Id; }
}

public int accountId;
        public uint playerId;
        public Types.PlayerStatus status;
        

public PlayerStatusUpdateMessage()
{
}

public PlayerStatusUpdateMessage(int accountId, uint playerId, Types.PlayerStatus status)
        {
            this.accountId = accountId;
            this.playerId = playerId;
            this.status = status;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteInt(accountId);
            writer.WriteVarUhInt(playerId);
            writer.WriteShort(status.TypeId);
            status.Serialize(writer);
            

}

public override void Deserialize(ICustomDataInput reader)
{

accountId = reader.ReadInt();
            if (accountId < 0)
                throw new Exception("Forbidden value on accountId = " + accountId + ", it doesn't respect the following condition : accountId < 0");
            playerId = reader.ReadVarUhInt();
            if (playerId < 0)
                throw new Exception("Forbidden value on playerId = " + playerId + ", it doesn't respect the following condition : playerId < 0");
            status = Types.ProtocolTypeManager.GetInstance<Types.PlayerStatus>(reader.ReadShort());
            status.Deserialize(reader);
            

}


}


}