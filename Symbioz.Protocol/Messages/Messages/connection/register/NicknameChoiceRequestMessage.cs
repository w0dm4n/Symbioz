


















// Generated on 06/04/2015 18:44:03
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class NicknameChoiceRequestMessage : Message
{

public const ushort Id = 5639;
public override ushort MessageId
{
    get { return Id; }
}

public string nickname;
        

public NicknameChoiceRequestMessage()
{
}

public NicknameChoiceRequestMessage(string nickname)
        {
            this.nickname = nickname;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteUTF(nickname);
            

}

public override void Deserialize(ICustomDataInput reader)
{

nickname = reader.ReadUTF();
            

}


}


}