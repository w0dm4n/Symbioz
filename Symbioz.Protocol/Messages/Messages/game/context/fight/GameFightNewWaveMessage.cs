


















// Generated on 06/04/2015 18:44:19
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class GameFightNewWaveMessage : Message
{

public const ushort Id = 6490;
public override ushort MessageId
{
    get { return Id; }
}

public sbyte id;
        public sbyte teamId;
        public short nbTurnBeforeNextWave;
        

public GameFightNewWaveMessage()
{
}

public GameFightNewWaveMessage(sbyte id, sbyte teamId, short nbTurnBeforeNextWave)
        {
            this.id = id;
            this.teamId = teamId;
            this.nbTurnBeforeNextWave = nbTurnBeforeNextWave;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteSByte(id);
            writer.WriteSByte(teamId);
            writer.WriteShort(nbTurnBeforeNextWave);
            

}

public override void Deserialize(ICustomDataInput reader)
{

id = reader.ReadSByte();
            if (id < 0)
                throw new Exception("Forbidden value on id = " + id + ", it doesn't respect the following condition : id < 0");
            teamId = reader.ReadSByte();
            if (teamId < 0)
                throw new Exception("Forbidden value on teamId = " + teamId + ", it doesn't respect the following condition : teamId < 0");
            nbTurnBeforeNextWave = reader.ReadShort();
            

}


}


}