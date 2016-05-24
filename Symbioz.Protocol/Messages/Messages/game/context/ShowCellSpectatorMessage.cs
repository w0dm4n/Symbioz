


















// Generated on 06/04/2015 18:44:19
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class ShowCellSpectatorMessage : ShowCellMessage
{

public const ushort Id = 6158;
public override ushort MessageId
{
    get { return Id; }
}

public string playerName;
        

public ShowCellSpectatorMessage()
{
}

public ShowCellSpectatorMessage(int sourceId, ushort cellId, string playerName)
         : base(sourceId, cellId)
        {
            this.playerName = playerName;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteUTF(playerName);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            playerName = reader.ReadUTF();
            

}


}


}