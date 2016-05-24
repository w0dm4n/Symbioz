


















// Generated on 06/04/2015 18:44:17
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class MoodSmileyRequestMessage : Message
{

public const ushort Id = 6192;
public override ushort MessageId
{
    get { return Id; }
}

public sbyte smileyId;
        

public MoodSmileyRequestMessage()
{
}

public MoodSmileyRequestMessage(sbyte smileyId)
        {
            this.smileyId = smileyId;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteSByte(smileyId);
            

}

public override void Deserialize(ICustomDataInput reader)
{

smileyId = reader.ReadSByte();
            

}


}


}