


















// Generated on 06/04/2015 18:44:04
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class AchievementFinishedInformationMessage : AchievementFinishedMessage
{

public const ushort Id = 6381;
public override ushort MessageId
{
    get { return Id; }
}

public string name;
        public uint playerId;
        

public AchievementFinishedInformationMessage()
{
}

public AchievementFinishedInformationMessage(ushort id, byte finishedlevel, string name, uint playerId)
         : base(id, finishedlevel)
        {
            this.name = name;
            this.playerId = playerId;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteUTF(name);
            writer.WriteVarUhInt(playerId);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            name = reader.ReadUTF();
            playerId = reader.ReadVarUhInt();
            if (playerId < 0)
                throw new Exception("Forbidden value on playerId = " + playerId + ", it doesn't respect the following condition : playerId < 0");
            

}


}


}