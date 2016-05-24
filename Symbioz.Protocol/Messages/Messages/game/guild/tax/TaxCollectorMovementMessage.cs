


















// Generated on 06/04/2015 18:44:47
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class TaxCollectorMovementMessage : Message
{

public const ushort Id = 5633;
public override ushort MessageId
{
    get { return Id; }
}

public bool hireOrFire;
        public Types.TaxCollectorBasicInformations basicInfos;
        public uint playerId;
        public string playerName;
        

public TaxCollectorMovementMessage()
{
}

public TaxCollectorMovementMessage(bool hireOrFire, Types.TaxCollectorBasicInformations basicInfos, uint playerId, string playerName)
        {
            this.hireOrFire = hireOrFire;
            this.basicInfos = basicInfos;
            this.playerId = playerId;
            this.playerName = playerName;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteBoolean(hireOrFire);
            basicInfos.Serialize(writer);
            writer.WriteVarUhInt(playerId);
            writer.WriteUTF(playerName);
            

}

public override void Deserialize(ICustomDataInput reader)
{

hireOrFire = reader.ReadBoolean();
            basicInfos = new Types.TaxCollectorBasicInformations();
            basicInfos.Deserialize(reader);
            playerId = reader.ReadVarUhInt();
            if (playerId < 0)
                throw new Exception("Forbidden value on playerId = " + playerId + ", it doesn't respect the following condition : playerId < 0");
            playerName = reader.ReadUTF();
            

}


}


}