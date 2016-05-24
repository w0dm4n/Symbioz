


















// Generated on 06/04/2015 18:45:18
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class KrosmasterPlayingStatusMessage : Message
{

public const ushort Id = 6347;
public override ushort MessageId
{
    get { return Id; }
}

public bool playing;
        

public KrosmasterPlayingStatusMessage()
{
}

public KrosmasterPlayingStatusMessage(bool playing)
        {
            this.playing = playing;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteBoolean(playing);
            

}

public override void Deserialize(ICustomDataInput reader)
{

playing = reader.ReadBoolean();
            

}


}


}