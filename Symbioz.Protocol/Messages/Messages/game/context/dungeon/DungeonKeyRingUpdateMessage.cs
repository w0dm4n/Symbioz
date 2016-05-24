


















// Generated on 06/04/2015 18:44:19
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class DungeonKeyRingUpdateMessage : Message
{

public const ushort Id = 6296;
public override ushort MessageId
{
    get { return Id; }
}

public ushort dungeonId;
        public bool available;
        

public DungeonKeyRingUpdateMessage()
{
}

public DungeonKeyRingUpdateMessage(ushort dungeonId, bool available)
        {
            this.dungeonId = dungeonId;
            this.available = available;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteVarUhShort(dungeonId);
            writer.WriteBoolean(available);
            

}

public override void Deserialize(ICustomDataInput reader)
{

dungeonId = reader.ReadVarUhShort();
            if (dungeonId < 0)
                throw new Exception("Forbidden value on dungeonId = " + dungeonId + ", it doesn't respect the following condition : dungeonId < 0");
            available = reader.ReadBoolean();
            

}


}


}